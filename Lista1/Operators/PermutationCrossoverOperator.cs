using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class PermutationCrossoverOperator : ICrossoverOperator
    {
        private static Random Random = new Random();
        public int ChildrenSize => 2;

        public int MaxNumber { get; set; }

        public PermutationCrossoverOperator(int maxNumber)
        {
            MaxNumber = maxNumber;
        }

        public Member[] Cross(Member first, Member second)
        {
            var child1 = first.DeepCopy();
            var child2 = second.DeepCopy();

            var number = Random.Next(0, MaxNumber) + 1;
            Swap(number, child1, child2);

            return new Member[] { child1, child2 };
        }

        private void Swap(int number, Member child1, Member child2)
        {
            // TODO: Swap
        }
    }
}
