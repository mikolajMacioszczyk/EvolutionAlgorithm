using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class RowMutation : IMutationOperator
    {
        private static Random random = new Random();
        public void Mutate(Member member)
        {
            var columnLenght = member.Matrix.GetLength(1);
            var row1 = random.Next(member.Matrix.GetLength(0));
            var row2 = random.Next(member.Matrix.GetLength(0));
            while (row1 == row2)
            {
                row2 = random.Next(member.Matrix.GetLength(0));
            }

            int temp;
            for (int i = 0; i < columnLenght; i++)
            {
                temp = member[row1, i];
                member[row1, i] = member[row2, i];
                member[row2, i] = temp;
            }
        }
    }
}
