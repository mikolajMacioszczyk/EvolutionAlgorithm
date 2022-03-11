using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class RandomReproductionOperator : IReproductionOperator
    {
        private static Random _random = new Random();
        private readonly ICrossoverOperator _crossoverOperator;

        public RandomReproductionOperator(ICrossoverOperator crossoverOperator)
        {
            _crossoverOperator = crossoverOperator;
        }

        public List<Member> GenerateChildren(List<Member> parents, int count)
        {
            var resultPopulation = new List<Member>(count + _crossoverOperator.ChildrenSize);

            while (resultPopulation.Count < count)
            {
                var parent1 = parents[_random.Next(parents.Count)];
                var parent2 = parents[_random.Next(parents.Count)];
                var children = _crossoverOperator.Cross(parent1, parent2);
                resultPopulation.AddRange(children);
            }

            return resultPopulation;
        }
    }
}
