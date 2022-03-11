using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class TestEvaluation : IEveluationOperator
    {
        public int Compare(Member? x, Member? y)
        {
            return Evaluate(x).CompareTo(Evaluate(y));
        }

        public double Evaluate(Member member)
        {
            var sum = 0;
            for (int i = 0; i < member.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < member.Matrix.GetLength(1); j++)
                {
                    sum += member[i, j] * (i + j);
                }
            }
            return sum;
        }
    }
}
