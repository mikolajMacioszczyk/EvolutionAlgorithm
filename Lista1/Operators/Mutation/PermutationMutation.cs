using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators.Mutation
{
    public class PermutationMutation : IMutationOperator
    {
        private static Random random = new Random();

        private int maxNumber;

        public PermutationMutation(int maxNumber)
        {
            this.maxNumber = maxNumber;
        }

        public void Mutate(Member member)
        {
            var permutation = GetPermutation();

            if (permutation.Length < 2)
            {
                return;
            }

            (int, int) oldCoordinates = member.GetCoordinatesOfNumber(permutation[0]);

            for (int i = 0; i < permutation.Length - 1; i++)
            {
                var coordinates = member.GetCoordinatesOfNumber(permutation[i + 1]);
                member[coordinates.Item1, coordinates.Item2] = permutation[i];
            }
            member[oldCoordinates.Item1, oldCoordinates.Item2] = permutation[permutation.Length - 1];
        }

        private int[] GetPermutation()
        {
            var permutationSize = random.Next(maxNumber);
            var permutation = new int[permutationSize];
            
            for (int i = 0; i < permutationSize; i++)
            {
                var number = random.Next(maxNumber) + 1;
                while (permutation.Contains(number))
                {
                    number = random.Next(maxNumber) + 1;
                }
                permutation[i] = number;
            }

            return permutation;
        }
    }
}
