﻿using Lista1.Interfaces;
using Lista1.Managers;
using Lista1.Models;
using Lista1.Operators;
using System.Text.Json;
using Lista1.Extensions;

namespace Lista1
{
    public class Program
    {
        const int maxRoundsWithoutProgress = 50;

        const int populationSize = 200;
        const int subPopulationSize = 1200;
        const int dimX = 5;
        const int dimY = 6;
        const int machinesCount = 24;

        const int baseDistance = 50;

        const double crossProbability = 0.3;
        const double eliteSize = 0.05;
        const int maxTournamentChampions = 3;
        const int treshold = 0;

        private int tournamentSize = 20;

        private IInitializationOperator initializationOperator;
        private ICrossoverOperator crossoverOperator;
        private IReproductionOperator reproductionOperator;
        private IMutationManager mutationManager;
        private IEveluationOperator eveluationOperator;
        private ISelectionOperator selectionOperator;
        private IAnnealingManager annealingManager;
        private IReportManager reportManager;

        public Program()
        {
            reportManager = new TextFileReportManager();
            //annealingManager = new LinearAnnealingManager(rounds, maxTournamentChampions);
            eveluationOperator = new ManhattanDistanceEvaluation(ReadFlowCostData(), machinesCount);
            //eveluationOperator = new PaddingDistanceEvaluation(ReadFlowCostData(), ReadMachinesPadding(), machinesCount, baseDistance);
            initializationOperator = new InitializationOperator();
            crossoverOperator = new CascadeCrossoverOperator(machinesCount, crossProbability);
            reproductionOperator = new RandomReproductionOperator(crossoverOperator);

            mutationManager = new MutationManager();
            mutationManager.RegisterOperator(new NoMutation(), 5, dimX, dimY, machinesCount); // brak mutacji
            mutationManager.RegisterOperator(new RectangleMovementMutation(dimX, dimY), 20, dimX, dimY, machinesCount); // przesunięcie prostokąta
            mutationManager.RegisterOperator(new CellMutation(), 15, dimX, dimY, machinesCount); // mutacja dwóch komórek
            mutationManager.RegisterOperator(new PermutationMutation(machinesCount), 5, dimX, dimY, machinesCount); // mutacja permutacyjna
            mutationManager.RegisterOperator(new RowMutation(), 2, dimX, dimY, machinesCount); // mutacja dwóch wierszy (preferowane ze względu na strukture pamięci)
            mutationManager.RegisterOperator(new ColumnMutation(), 2, dimX, dimY, machinesCount); // mutacja dwóch kolumn
            mutationManager.RegisterOperator(new ReverseColumnMutation(), 1, dimX, dimY, machinesCount); // odrócenie kolumny
            mutationManager.RegisterOperator(new ReverseRowMutation(), 1, dimX, dimY, machinesCount); // odrócenie kolumny

            //selectionOperator = new SimpleTournamentSelectionOperator(eveluationOperator, tournamentSize);
            selectionOperator = new RouletteSelectionOperator(eveluationOperator, (int)Math.Round(populationSize * eliteSize));
        }

        private void Run(Report report)
        {
            report.SelectionOperator = selectionOperator.Name;
            report.EvaluationOperator = eveluationOperator.Name;

            var startTime = DateTime.Now;
            var population = initializationOperator.InitializePopulation(populationSize, dimX, dimY, machinesCount);
            Console.WriteLine($"Initialized population with {populationSize} members.");
            Console.WriteLine($"Each member consist of {dimX} x {dimY} grid on which {machinesCount} mechines are randomly placed");

            double bestResult = double.MaxValue;
            int round = 0;
            int roundsWithoutProgress = 0;
            while (roundsWithoutProgress < maxRoundsWithoutProgress && bestResult > treshold)
            {
                round++;
                roundsWithoutProgress++;

                // select elite
                var eliteCount = (int)Math.Round(population.Count * eliteSize);
                var elite = population.OrderBy(m => eveluationOperator.Evaluate(m)).Take(eliteCount).ToList();

                // generation
                population = reproductionOperator.GenerateChildren(population, subPopulationSize - eliteCount);

                // mutation
                mutationManager.MutatePopulation(population);

                population.AddRange(elite);

                // selection
                population = selectionOperator.Select(populationSize, population, round);

                // show statistics and check if progress
                var newBest = CollectStatis(population, report, round);
                if (newBest < bestResult)
                {
                    bestResult = newBest;
                    roundsWithoutProgress = 0;
                }
            }
            report.Time = DateTime.Now - startTime;
            report.MutationOperatorsInfo = mutationManager.GetOparatorsInfo();

            if (!report.RoundStats.Any())
            {
                CollectStatis(population, report, round);
            }

            var resutBest = report.RoundStats.LastOrDefault()?.Best;
            report.BestMember = population.First(m => eveluationOperator.Evaluate(m) == resutBest);
            reportManager.Save(report);
            Console.WriteLine($"Done in {report.Time.Milliseconds} ms");
            Console.WriteLine($"Best result: {bestResult}");
            PrintMember(report.BestMember);
            Console.ReadLine();
        }

        static void Main()
        {
            if (populationSize < 1 || subPopulationSize < 1)
            {
                Console.WriteLine("Population and SubPopulation size must be greater then 0!");
                return;
            }
            if (subPopulationSize % populationSize > 0)
            {
                Console.WriteLine("inefficiently selected parameters.\n" +
                    "\"subPopulationSize\" should be divisable by \"populationSize\"");
            }

            var report = new Report()
            {
                MaxRoundsWithoutProgress = maxRoundsWithoutProgress,
                DimX = dimX,
                DimY = dimY,
                CrossProbability = crossProbability,
                EliteSize = eliteSize,
                MachinesCount = machinesCount,
                MaxTournamentChampions = maxTournamentChampions,
                PopulationSize = populationSize,
                SubPopulationSize = subPopulationSize,
                RoundStats = new List<RoundStats>(),
            };

            new Program().Run(report);
        }

        private double CollectStatis(IEnumerable<Member> population, Report report, int round)
        {
            double bestResult = population.Min(eveluationOperator.Evaluate);
            double worstResult = population.Max(eveluationOperator.Evaluate);
            double averageResult = population.Average(eveluationOperator.Evaluate);
            double std = population.StdDev(m => eveluationOperator.Evaluate(m));

            report.RoundStats.Add(new RoundStats { Best = bestResult, Worst = worstResult, Average = averageResult, Std = std });

            Console.WriteLine($"Round {round}: best = {bestResult}, worst = {worstResult}, average = {averageResult}, std = {std}");
            return bestResult;
        }

        private Dictionary<int, Dictionary<int, int>> ReadFlowCostData()
        {
            var (costs, flows) = GetInpudData();

            var result = new Dictionary<int, Dictionary<int, int>>(){};

            foreach (var cost in costs)
            {
                if (!result.ContainsKey(cost.Source))
                {
                    result.Add(cost.Source, new Dictionary<int, int>());
                }

                result[cost.Source].Add(cost.Dest, cost.Cost);
            }

            foreach (var flow in flows)
            {
                result[flow.Source][flow.Dest] *= flow.Amount;
            }

            return result;
        }

        private Dictionary<int, int> ReadMachinesPadding()
        {
            var root = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
            var input = Path.Combine(root, "Input");
            var paddingJson = Path.Join(input, "padding.json");

            if (!File.Exists(paddingJson))
            {
                throw new Exception("File Input/padding.json not exist");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = new Dictionary<int, int>() { };

            var paddingsString = File.ReadAllText(paddingJson);
            var paddings = JsonSerializer.Deserialize<IEnumerable<PaddingModel>>(paddingsString, options);
            foreach (var padding in paddings)
            {
                padding.Source += 1;
                result.Add(padding.Source, padding.Padding);
            }

            return result;
        }

        private (IEnumerable<CostModel>, IEnumerable<FlowModel>) GetInpudData()
        {
            var root = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
            var input = Path.Combine(root, "Input");
            var costJson = Path.Join(input, "cost.json");
            var flowJson = Path.Join(input, "flow.json");

            if (!File.Exists(costJson))
            {
                throw new Exception("File Input/cost.json not exist");
            }
            if (!File.Exists(flowJson))
            {
                throw new Exception("File Input/flow.json not exist");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var costString = File.ReadAllText(costJson);
            var costs = JsonSerializer.Deserialize<IEnumerable<CostModel>>(costString, options);
            foreach (var cost in costs)
            {
                cost.Source += 1;
                cost.Dest += 1;
            }

            var flowString = File.ReadAllText(flowJson);
            var flows = JsonSerializer.Deserialize<IEnumerable<FlowModel>>(flowString, options);
            foreach (var flow in flows)
            {
                flow.Source += 1;
                flow.Dest += 1;
            }

            return (costs, flows);
        }

        private void PrintMember(Member member)
        {
            Console.WriteLine();
            Console.WriteLine($"{member.Matrix.GetLength(0)} x {member.Matrix.GetLength(1)}");
            for (int i = 0; i < member.Matrix.GetLength(0); i++)
            {
                Console.WriteLine(new string('-', member.Matrix.GetLength(1) * 4));
                for (int j = 0; j < member.Matrix.GetLength(1); j++)
                {
                    var value = member.Matrix[i, j];
                    if (value > 0)
                    {
                        Console.Write($"{value - 1, 2} ");
                    }
                    else
                    {
                        Console.Write($"{"*", 2} ");
                    }
                    Console.Write('|');
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', member.Matrix.GetLength(1) * 4));
        }
    }
}