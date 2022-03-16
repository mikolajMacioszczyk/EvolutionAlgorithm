using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class ReverseRowMutation : IMutationOperator
    {
        private static Random random = new Random();
        public string Name => nameof(ReverseRowMutation);

        public bool CanRegister(int dimX, int dimY, int machinesCount) => dimX > 0 && dimY > 1;

        public void Mutate(Member member)
        {
            var row = random.Next(member.Matrix.GetLength(0));

            var copy = new int[member.Matrix.GetLength(1)];
            for (var i = 0; i < copy.Length; i++)
            {
                copy[i] = member[row, i];
            }

            for (int i = 0; i < copy.Length; i++)
            {
                member[row, i] = copy[copy.Length - 1 - i];
            }
        }
    }
}
