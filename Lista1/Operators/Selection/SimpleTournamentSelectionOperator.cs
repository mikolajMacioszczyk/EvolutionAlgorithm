using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class SimpleTournamentSelectionOperator : ISelectionOperator
    {
        private static Random _random = new Random();
        private readonly IEveluationOperator _evaluationOperator;

        public SimpleTournamentSelectionOperator(IEveluationOperator evaluationOperator)
        {
            _evaluationOperator = evaluationOperator;
        }

        public List<Member> Select(int count, List<Member> source)
        {
            // TODO: wyżażanie
            var rest = source.Count % count;
            if (rest > 0)
            {
                for (int i = 0; i < count - rest; i++)
                {
                    source.Add(source[_random.Next(source.Count)].DeepCopy());
                }
            }

            var result = new List<Member>(count);

            var tournamentSize = source.Count / count;

            // shuffle
            var sourceArray = source.OrderBy(x => _random.Next()).ToArray();

            for (int i = 0; i < source.Count; i += tournamentSize)
            {
                Array.Sort(sourceArray, i, tournamentSize, _evaluationOperator);

                // get best from tournament
                result.Add(sourceArray[i]);
            }

            return result;
        }
    }
}
