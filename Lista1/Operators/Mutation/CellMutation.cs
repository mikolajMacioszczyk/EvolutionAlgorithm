using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class CellMutation : IMutationOperator
    {
        private static Random random = new Random();
        public void Mutate(Member member)
        {
            var cell1X = random.Next(member.Matrix.GetLength(0));
            var cell1Y = random.Next(member.Matrix.GetLength(1));

            var cell2X = random.Next(member.Matrix.GetLength(0));
            var cell2Y = random.Next(member.Matrix.GetLength(1));

            var temp = member[cell1X, cell1Y];
            member[cell1X, cell1Y] = member[cell2X, cell2Y];
            member[cell2X, cell2Y] = temp;
        }
    }
}
