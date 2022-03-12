using Lista1.Interfaces;
using Lista1.Managers;
using Lista1.Models;
using Lista1.Operators;
using System.Text.Json;

namespace Lista1
{
    public class Program
    {
        const int rounds = 200;

        const int populationSize = 50;
        const int subPopulationSize = 300;
        const int dimX = 5;
        const int dimY = 6;
        const int machinesCount = 24;

        const double crossWish = 0.3;
        const double eliteSize = 0.05;
        const int maxTournamentChampions = 3;

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
            annealingManager = new LinearAnnealingManager(rounds, maxTournamentChampions);
            eveluationOperator = new ManhattanDistanceEvaluation(ReadFlowCostData(), machinesCount);
            initializationOperator = new InitializationOperator();
            crossoverOperator = new CascadeCrossoverOperator(machinesCount, crossWish);
            reproductionOperator = new RandomWithEliteReporoductionOperator(crossoverOperator, eveluationOperator, eliteSize);

            mutationManager = new MutationManager();
            mutationManager.RegisterOperator(new NoMutation(), 20); // brak mutacji
            mutationManager.RegisterOperator(new CellMutation(), 20); // mutacja dwóch komórek
            mutationManager.RegisterOperator(new RectangleMovementMutation(dimX, dimY), 5); // przesunięcie prostokąta
            mutationManager.RegisterOperator(new RowMutation(), 2); // mutacja dwóch wierszy (preferowane ze względu na strukture pamięci)
            mutationManager.RegisterOperator(new ColumnMutation(), 2); // mutacja dwóch kolumn
            mutationManager.RegisterOperator(new PermutationMutation(machinesCount), 2); // mutacja permutacyjna

            selectionOperator = new SimpleTournamentSelectionOperator(eveluationOperator);
        }

        private void Run(Report report)
        {
            var population = initializationOperator.InitializePopulation(populationSize, dimX, dimY, machinesCount);
            Console.WriteLine($"Initialized population with {populationSize} members.");
            Console.WriteLine($"Each member consist of {dimX} x {dimY} grid on which {machinesCount} mechines are randomly placed");

            for (int i = 0; i < rounds; i++)
            {
                // TODO: cross based on relationship
                // generation
                population = reproductionOperator.GenerateChildren(population, subPopulationSize);
                
                // mutation
                mutationManager.MutatePopulation(population);

                // selection
                population = selectionOperator.Select(populationSize, population, i);

                // show statistics
                CollectStatis(population, report, i);
            }

            var bestResult = report.RoundStats.LastOrDefault()?.Best;
            report.BestMember = population.FirstOrDefault(m => eveluationOperator.Evaluate(m) == bestResult);
            reportManager.Save(report);
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

            var report = new Report()
            {
                Rounds = rounds,
                DimX = dimX,
                DimY = dimY,
                CrossWish = crossWish,
                EliteSize = eliteSize,
                MachinesCount = machinesCount,
                MaxTournamentChampions = maxTournamentChampions,
                PopulationSize = populationSize,
                SubPopulationSize = subPopulationSize,
                RoundStats = new List<RoundStats>()
            };

            new Program().Run(report);
        }

        private void CollectStatis(IEnumerable<Member> population, Report report, int round)
        {
            double bestResult = population.Min(eveluationOperator.Evaluate);
            double worstResult = population.Max(eveluationOperator.Evaluate);
            double averageResult = population.Average(eveluationOperator.Evaluate);

            report.RoundStats.Add(new RoundStats { Best = bestResult, Worst = worstResult, Average = averageResult });

            Console.WriteLine($"Round {round}: best = {bestResult}, worst = {worstResult}, average = {averageResult}");
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