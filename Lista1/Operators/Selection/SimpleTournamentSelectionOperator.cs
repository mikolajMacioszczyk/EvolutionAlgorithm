using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class SimpleTournamentSelectionOperator : ISelectionOperator
    {
        private static Random _random = new Random();
        private readonly IEveluationOperator _evaluationOperator;
        private readonly int _tournamentSize;

        public SimpleTournamentSelectionOperator(IEveluationOperator evaluationOperator, int tournamentSize)
        {
            _evaluationOperator = evaluationOperator;
            _tournamentSize = tournamentSize;
        }

        public List<Member> Select(int count, List<Member> source, int currentRound)
        {
            var result = new List<Member>(count);

            var rest = (source.Count % _tournamentSize);
            if (rest > 0)
            {
                for (int i = 0; i < _tournamentSize - rest; i++)
                {
                    source.Add(source[_random.Next(source.Count)].DeepCopy());
                }
            }

            // shuffle
            var sourceArray = source.OrderBy(x => _random.Next()).ToArray();

            for (int i = 0; i < source.Count; i += _tournamentSize)
            {
                Array.Sort(sourceArray, i, _tournamentSize, _evaluationOperator);

                // get best from tournament
                result.Add(sourceArray[i]);
            }

            return result;
        }
    }
}
