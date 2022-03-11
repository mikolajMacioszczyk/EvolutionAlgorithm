using Lista1.Interfaces;
using Lista1.Managers;
using Lista1.Models;
using Lista1.Operators;
using System.Text.Json;

namespace Lista1
{
    public class Program
    {
        const int rounds = 500;

        const int populationSize = 50;
        const int subPopulationSize = 300;
        const int dimX = 5;
        const int dimY = 6;
        const int machinesCount = 24;

        const double crossWish = 0.3;
        const double eliteSize = 0.05;

        private IInitializationOperator initializationOperator;
        private ICrossoverOperator crossoverOperator;
        private IReproductionOperator reproductionOperator;
        private IMutationManager mutationManager;
        private IEveluationOperator eveluationOperator;
        private ISelectionOperator selectionOperator;

        public Program()
        {
            eveluationOperator = new ManhattanDistanceEvaluation(ReadFlowCostData(), machinesCount);
            initializationOperator = new InitializationOperator();
            crossoverOperator = new CascadeCrossoverOperator(machinesCount, crossWish);
            reproductionOperator = new RandomWithEliteReporoductionOperator(crossoverOperator, eveluationOperator, eliteSize);

            mutationManager = new MutationManager();
            mutationManager.RegisterOperator(new NoMutation(), 20); // brak mutacji
            mutationManager.RegisterOperator(new CellMutation(), 20); // mutacja dwóch komórek
            mutationManager.RegisterOperator(new RowMutation(), 4); // mutacja dwóch wierszy (preferowane ze względu na strukture pamięci)
            mutationManager.RegisterOperator(new ColumnMutation(), 2); // mutacja dwóch kolumn
            mutationManager.RegisterOperator(new PermutationMutation(machinesCount), 1); // mutacja permutacyjna

            selectionOperator = new SimpleTournamentSelectionOperator(eveluationOperator);
        }

        private void Run()
        {
            var population = initializationOperator.InitializePopulation(populationSize, dimX, dimY, machinesCount);
            Console.WriteLine($"Initialized population with {populationSize} members.");
            Console.WriteLine($"Each member consist of {dimX} x {dimY} grid on which {machinesCount} mechines are randomly placed");

            for (int i = 0; i < rounds; i++)
            {
                population = reproductionOperator.GenerateChildren(population, subPopulationSize);
                mutationManager.MutatePopulation(population);

                // wypisz statystyki

                // todo: Wyzażanie
                population = selectionOperator.Select(populationSize, population);

                // temp
                var bestResult = population.Min(eveluationOperator.Evaluate);
                var worstResult = population.Max(eveluationOperator.Evaluate);
                var averageResult = population.Average(eveluationOperator.Evaluate);

                Console.WriteLine($"Round {i}: best = {bestResult}, worst = {worstResult}, average = {averageResult}");
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static void Main()
        {
            if (subPopulationSize % populationSize > 0)
            {
                Console.WriteLine("inefficiently selected parameters.\n" +
                    "\"subPopulationSize\" should be divisable by \"populationSize\"");
            }
            new Program().Run();
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
    }
}