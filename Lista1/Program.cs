using Lista1.Operators;

namespace Lista1
{
    static class Program
    {
        const int rounds = 100;

        const int populationSize = 10;
        const int dimX = 3;
        const int dimY = 4;
        const int machinesCount = 9;

        static void Main()
        {
            var population = InitializationOperator.InitializePopulation(populationSize, dimX, dimY, machinesCount);
            Console.WriteLine($"Initialized population with {populationSize} members.");
            Console.WriteLine($"Each member consist of {dimX} x {dimY} grid on which {machinesCount} mechines are randomly placed");

            Console.ReadLine();
        }
    }
}