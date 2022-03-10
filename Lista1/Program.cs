using Lista1.Interfaces;
using Lista1.Operators;

namespace Lista1
{
    public class Program
    {
        const int rounds = 100;

        const int populationSize = 10;
        const int dimX = 3;
        const int dimY = 4;
        const int machinesCount = 9;

        private IInitializationOperator initializationOperator;

        public Program()
        {
            initializationOperator = new InitializationOperator();
        }

        private void Run()
        {
            var population = initializationOperator.InitializePopulation(populationSize, dimX, dimY, machinesCount);
            Console.WriteLine($"Initialized population with {populationSize} members.");
            Console.WriteLine($"Each member consist of {dimX} x {dimY} grid on which {machinesCount} mechines are randomly placed");

            for (int i = 0; i < rounds; i++)
            {
                // krzyżuj
                // mutuj (może)
                // posortuj
                // wybierz populacje dzieci
            }

            Console.ReadLine();
        }

        static void Main()
        {
            new Program().Run();
        }
    }
}