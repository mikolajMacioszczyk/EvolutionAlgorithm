using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class AnnealingTournamentSelectionOperator : ISelectionOperator
    {
        private static Random _random = new Random();
        private readonly IEveluationOperator _evaluationOperator;
        private readonly IAnnealingManager _annealingManager;

        public AnnealingTournamentSelectionOperator(IEveluationOperator evaluationOperator, IAnnealingManager annealingManager)
        {
            _evaluationOperator = evaluationOperator;
            _annealingManager = annealingManager;
        }

        public string Name => nameof(AnnealingTournamentSelectionOperator);

        public List<Member> Select(int count, List<Member> source, int currentRound)
        {
            int champions = _annealingManager.GetChampionsCount(currentRound);

            var result = new List<Member>(count);

            var tournamentSize = (source.Count / count) * champions;
            var rest = (source.Count % tournamentSize);
            if (rest > 0)
            {
                for (int i = 0; i < tournamentSize - rest; i++)
                {
                    source.Add(source[_random.Next(source.Count)].DeepCopy());
                }
            }

            // shuffle
            var sourceArray = source.OrderBy(x => _random.Next()).ToArray();

            for (int i = 0; i < source.Count; i += tournamentSize)
            {
                Array.Sort(sourceArray, i, tournamentSize, _evaluationOperator);

                // get best from tournament
                for (int j = 0; j < champions; j++)
                {
                    result.Add(sourceArray[i + j]);
                }
            }

            return result;
        }
    }
}
