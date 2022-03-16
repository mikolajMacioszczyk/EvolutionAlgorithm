using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class ReverseColumnMutation : IMutationOperator
    {
        private static Random random = new Random();
        public string Name => nameof(ReverseColumnMutation);

        public bool CanRegister(int dimX, int dimY, int machinesCount)
        {
            return dimX > 1 && dimY > 0;
        }

        public void Mutate(Member member)
        {
            var column = random.Next(member.Matrix.GetLength(1));

            var copy = new int[member.Matrix.GetLength(0)];
            for (var i = 0; i < copy.Length; i++)
            {
                copy[i] = member[i, column];
            }

            for (int i = 0; i < copy.Length; i++)
            {
                member[i, column] = copy[copy.Length - 1 - i];
            }
        }
    }
}
