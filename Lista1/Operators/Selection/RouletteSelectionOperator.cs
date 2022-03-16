using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class RouletteSelectionOperator : ISelectionOperator
    {
        private static Random _random = new Random();
        private readonly IEveluationOperator _evaluationOperator;
        private readonly int _eliteSize;

        public RouletteSelectionOperator(IEveluationOperator evaluationOperator, int eliteSize)
        {
            _evaluationOperator = evaluationOperator;
            _eliteSize = eliteSize;
        }

        public string Name => nameof(RouletteSelectionOperator);

        public List<Member> Select(int count, List<Member> source, int currentRound)
        {
            double sum = 0;

            var result = source.OrderBy(m => _evaluationOperator.Evaluate(m)).Take(_eliteSize).ToList();

            var roulette = new List<(double, Member, bool)>();

            foreach (var member in source.Except(result))
            {
                var value = 1.0 / Math.Sqrt(_evaluationOperator.Evaluate(member));
                sum += value;
                roulette.Add((sum, member, false));
            }

            while (result.Count < count)
            {
                var choice = _random.NextDouble() * sum;
                var selected = roulette.FirstOrDefault(kv => kv.Item1 > choice);
                while (selected.Item3)
                {
                    choice = _random.NextDouble() * sum;
                    selected = roulette.FirstOrDefault(kv => kv.Item1 > choice);
                }
                selected.Item3 = true;
                result.Add(selected.Item2);
            }

            return result;
        }
    }
}
