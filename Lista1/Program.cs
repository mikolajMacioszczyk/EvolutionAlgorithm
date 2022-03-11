using Lista1.Interfaces;
using Lista1.Managers;
using Lista1.Operators;
using Lista1.Operators.Mutation;

namespace Lista1
{
    public class Program
    {
        const int rounds = 100;

        const int populationSize = 10;
        const int subPopulationSize = 26;
        const int dimX = 3;
        const int dimY = 5;
        const int machinesCount = 9;

        private IInitializationOperator initializationOperator;
        private ICrossoverOperator crossoverOperator;
        private IReproductionOperator reproductionOperator;
        private IMutationManager mutationManager;

        public Program()
        {
            initializationOperator = new InitializationOperator();
            crossoverOperator = new PermutationCrossoverOperator(machinesCount);
            reproductionOperator = new RandomReproductionOperator(crossoverOperator);

            mutationManager = new MutationManager();
            mutationManager.RegisterOperator(new NoMutation(), 20); // brak mutacji
            mutationManager.RegisterOperator(new RowMutation(), 5); // mutacja dwóch wierszy (preferowane ze względu na strukture pamięci)
            mutationManager.RegisterOperator(new ColumnMutation(), 2); // mutacja dwóch kolumn
            mutationManager.RegisterOperator(new CellMutation(), 1); // mutacja dwóch komórek
            mutationManager.RegisterOperator(new PermutationMutation(machinesCount), 1); // mutacja permutacyjna
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
                // posortuj
                // wybierz populacje dzieci
                // wypisz statystyki
            }

            // wybierz najlepsze rozwiązanie
            Console.WriteLine("Done");

            Console.ReadLine();
        }

        static void Main()
        {
            new Program().Run();
        }
    }
}