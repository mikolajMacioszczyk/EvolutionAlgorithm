using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class PaddingDistanceEvaluation : IEveluationOperator
    {
        private readonly Dictionary<int, Dictionary<int, int>> _costsOfFlow;
        private readonly Dictionary<int, int> _machinePadding;
        private readonly int _maxValue;
        private readonly int _baseDistance;

        public PaddingDistanceEvaluation(Dictionary<int, Dictionary<int, int>> costOfFlow, Dictionary<int, int> padding, int maxValue, int baseDistance)
        {
            _costsOfFlow = costOfFlow;
            _maxValue = maxValue;
            _machinePadding = padding;
            _baseDistance = baseDistance;
        }

        public int Compare(Member? x, Member? y)
        {
            return Evaluate(x).CompareTo(Evaluate(y));
        }

        public double Evaluate(Member member)
        {
            if (member.Evaluation.HasValue)
            {
                return member.Evaluation.Value;
            }

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
                        
                        //var distance = Math.Abs(iCoords.Item1 - jCoords.Item1) + Math.Abs(iCoords.Item2 - jCoords.Item2);
                        var distance = Math.Max(Math.Abs(iCoords.Item1 - jCoords.Item1), Math.Abs(iCoords.Item2 - jCoords.Item2));

                        // both machiness cumulated padding
                        int padding = _machinePadding[i] + _machinePadding[j];

                        totalFlowClost *= distance * _baseDistance + padding;

                        sum += totalFlowClost;
                    }
                }
            }

            member.Evaluation = sum;

            return sum;
        }
    }
}
