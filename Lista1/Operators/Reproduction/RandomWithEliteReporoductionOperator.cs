using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    internal class RandomWithEliteReporoductionOperator : IReproductionOperator
    {
        private static Random _random = new Random();
        private readonly ICrossoverOperator _crossoverOperator;
        private readonly IEveluationOperator _evaluationOperator;
        private readonly double _elitePart;

        public RandomWithEliteReporoductionOperator(ICrossoverOperator crossoverOperator, IEveluationOperator evaluationOperator, double elitePart)
        {
            _crossoverOperator = crossoverOperator;
            _elitePart = Math.Min(1, Math.Max(0, elitePart));
            _evaluationOperator = evaluationOperator;
        }

        public List<Member> GenerateChildren(List<Member> parents, int count)
        {
            var resultPopulation = new List<Member>(count + _crossoverOperator.ChildrenSize);

            var elite = (int) Math.Round(parents.Count * _elitePart);
            // need to be sorted
            resultPopulation.AddRange(parents.OrderBy(m => _evaluationOperator.Evaluate(m)).Take(elite));

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
