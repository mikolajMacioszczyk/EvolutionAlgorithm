using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class InitializationOperator : IInitializationOperator
    {
        private static Random Random = new Random();

        public List<Member> InitializePopulation(int populationSize, int dimX, int dimY, int machinesCount)
        {
            if (dimX * dimY < machinesCount)
            {
                throw new ArgumentException("Cannot initialize population: dimX * dimY < machinesCount");
            }

            var result = new List<Member>(populationSize);
            bool located;

            for (int i = 0; i < populationSize; i++)
            {
                var member = new Member(dimX, dimY);
                for (int j = 0; j < machinesCount; j++)
                {
                    located = false;
                    while (!located)
                    {
                        var x = Random.Next(0, dimX);
                        var y = Random.Next(0, dimX);

                        if (member[x, y] <= 0)
                        {
                            member[x, y] = j + 1;
                            located = true;
                        }
                    }

                }
                result.Add(member);
            }

            return result;
        }
    }
}
