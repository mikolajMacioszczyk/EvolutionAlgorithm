using Lista1.Interfaces;
using Lista1.Managers;
using Lista1.Operators;

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
            }

            // result
            var result = population.ToArray();
            Array.Sort(result, eveluationOperator);

            // wybierz najlepsze rozwiązanie
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
            return new Dictionary<int, Dictionary<int, int>>()
            {
            };
        }
    }
}