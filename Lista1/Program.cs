using Lista1.Interfaces;
using Lista1.Operators;

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

        public Program()
        {
            initializationOperator = new InitializationOperator();
            crossoverOperator = new PermutationCrossoverOperator(machinesCount);
            reproductionOperator = new RandomReproductionOperator(crossoverOperator);
        }

        private void Run()
        {
            var population = initializationOperator.InitializePopulation(populationSize, dimX, dimY, machinesCount);
            Console.WriteLine($"Initialized population with {populationSize} members.");
            Console.WriteLine($"Each member consist of {dimX} x {dimY} grid on which {machinesCount} mechines are randomly placed");

            for (int i = 0; i < rounds; i++)
            {
                population = reproductionOperator.GenerateChildren(population, subPopulationSize);
                // krzyżuj
                // mutuj (może)
                    // mutacja dwóch komórek
                    // mutacja dwóch wierszy (preferowane ze względu na strukture pamięci)
                    // mutacja dwóch kolumn
                    // mutacja permutacyjna
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