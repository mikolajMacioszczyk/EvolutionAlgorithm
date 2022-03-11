using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    // czy można używać intów?
    // czy musimy się martwić tym, że użytkownik może podać niepoprane dane?
    // czy można od razu pomnożyć koszt z flow?
    // czy koszt przepływu jest w dwie strony czy A -> B != B -> A?
    // jak liczyć odległość?
    // czy to dobrze że pomijam część możliwości między maszynami?
    public class ManhattanDistanceEvaluation : IEveluationOperator
    {
        private readonly Dictionary<int, Dictionary<int, int>> _costsOfFlow;
        private readonly int _maxValue;

        public ManhattanDistanceEvaluation(Dictionary<int, Dictionary<int, int>> costOfFlow, int maxValue)
        {
            _costsOfFlow = costOfFlow;
            _maxValue = maxValue;
        }

        public int Compare(Member? x, Member? y)
        {
            return Evaluate(x).CompareTo(Evaluate(y));
        }

        public double Evaluate(Member member)
        {
            var sum = 0;

            for (int i = 1; i <= _maxValue; i++)
            {
                for (int j = i + 1; j <= _maxValue; j++)
                {
                    var iCoords = member.GetCoordinatesOfNumber(i);
                        
                    var jCoords = member.GetCoordinatesOfNumber(j);

                    if (_costsOfFlow[i].ContainsKey(j))
                    {
                        var totalFlowClost = _costsOfFlow[i][j];
                        var distance = Math.Abs(iCoords.Item1 - jCoords.Item1) + Math.Abs(iCoords.Item2 - jCoords.Item2);
                        totalFlowClost *= distance;

                        sum += totalFlowClost;
                    }
                }
            }

            return sum;
        }
    }
}
