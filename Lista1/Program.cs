using Lista1.Interfaces;
using Lista1.Managers;
using Lista1.Models;
using Lista1.Operators;
using System.Text.Json;

namespace Lista1
{
    public class Program
    {
        const int rounds = 100;

        const int populationSize = 10;
        const int subPopulationSize = 30;
        const int dimX = 3;
        const int dimY = 5;
        const int machinesCount = 9;

        const double crossWish = 0.5;

        private IInitializationOperator initializationOperator;
        private ICrossoverOperator crossoverOperator;
        private IReproductionOperator reproductionOperator;
        private IMutationManager mutationManager;
        private IEveluationOperator eveluationOperator;
        private ISelectionOperator selectionOperator;

        public Program()
        {
            initializationOperator = new InitializationOperator();
            crossoverOperator = new CascadeCrossoverOperator(machinesCount, crossWish);
            reproductionOperator = new RandomReproductionOperator(crossoverOperator);

            mutationManager = new MutationManager();
            mutationManager.RegisterOperator(new NoMutation(), 25); // brak mutacji
            mutationManager.RegisterOperator(new CellMutation(), 10); // mutacja dwóch komórek
            mutationManager.RegisterOperator(new RowMutation(), 4); // mutacja dwóch wierszy (preferowane ze względu na strukture pamięci)
            mutationManager.RegisterOperator(new ColumnMutation(), 2); // mutacja dwóch kolumn
            mutationManager.RegisterOperator(new PermutationMutation(machinesCount), 1); // mutacja permutacyjna

            eveluationOperator = new ManhattanDistanceEvaluation(ReadFlowCostData(), machinesCount);
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
                var bestResult = population.Max(eveluationOperator.Evaluate);
                Console.WriteLine($"Round {i}: best member = {bestResult}");
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

                if (!result.ContainsKey(cost.Dest))
                {
                    result.Add(cost.Dest, new Dictionary<int, int>());
                }

                result[cost.Dest].Add(cost.Source, cost.Cost);
            }

            foreach (var flow in flows)
            {
                result[flow.Source][flow.Dest] *= flow.Amount;
                result[flow.Dest][flow.Source] *= flow.Amount;
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