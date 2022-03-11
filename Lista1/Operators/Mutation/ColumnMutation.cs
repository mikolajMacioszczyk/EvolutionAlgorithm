using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class ColumnMutation : IMutationOperator
    {
        private static Random random = new Random();

        public void Mutate(Member member)
        {
            var rowLenght = member.Matrix.GetLength(0);
            var col1 = random.Next(member.Matrix.GetLength(1));
            var col2 = random.Next(member.Matrix.GetLength(1));
            while (col1 == col2)
            {
                col2 = random.Next(member.Matrix.GetLength(1));
            }

            int temp;
            for (int i = 0; i < rowLenght; i++)
            {
                temp = member[i, col1];
                member[i, col1] = member[i, col2];
                member[i, col2] = temp;
            }
        }
    }
}
