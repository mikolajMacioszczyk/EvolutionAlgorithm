using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class CascadeCrossoverOperator : ICrossoverOperator
    {
        private static Random Random = new Random();
        public int ChildrenSize => 2;
        
        private double CrossProbability { get; set; }

        public int MaxNumber { get; set; }

        public CascadeCrossoverOperator(int maxNumber, double crossProbability)
        {
            MaxNumber = maxNumber;
            CrossProbability = crossProbability;
        }

        public Member[] Cross(Member first, Member second)
        {
            var child1 = first.DeepCopy();
            var child2 = second.DeepCopy();

            var number = Random.Next(0, MaxNumber) + 1;

            PerformCross(number, child1, child2);

            return new Member[] { child1, child2 };
        }

        private void PerformCross(int number, Member firstChild, Member secondChild)
        {
            var coords = firstChild.GetCoordinatesOfNumber(number);
            var exchangedNumber = secondChild[coords.Item1, coords.Item2];
            
            // no change
            if (number == exchangedNumber)
            {
                return;
            }

            // empty field
            if (exchangedNumber == 0)
            {
                SwapWithEmptyField(number, firstChild, secondChild, coords);
            }
            else
            {
                SwapWithValues(number, exchangedNumber, firstChild, secondChild, coords);
            }
        }

        private void SwapWithValues(int number, int exchangedNumber, Member firstChild, Member secondChild, (int, int) coords)
        {
            // swap values
            var newCoords = firstChild.GetCoordinatesOfNumber(exchangedNumber);
            firstChild[coords.Item1, coords.Item2] = exchangedNumber;
            firstChild[newCoords.Item1, newCoords.Item2] = number;

            var newCoorsd2 = secondChild.GetCoordinatesOfNumber(number);
            secondChild[coords.Item1, coords.Item2] = number;
            secondChild[newCoorsd2.Item1, newCoorsd2.Item2] = exchangedNumber;

            if (Random.NextDouble() < CrossProbability)
            {
                // recurence with the same values
                // bubbles starting values
                PerformCross(number, firstChild, secondChild);
            }
        }

        private void SwapWithEmptyField(int number, Member firstChild, Member secondChild, (int, int) coords)
        {
            firstChild[coords.Item1, coords.Item2] = 0;

            // find random zero-value position
            var zeroCoords = GetRandomEmptyPosition(firstChild);
            // swap with that position
            firstChild[zeroCoords.Item1, zeroCoords.Item2] = number;

            // second
            var passiveCoords = secondChild.GetCoordinatesOfNumber(number);
            secondChild[coords.Item1, coords.Item2] = number;
            secondChild[passiveCoords.Item1, passiveCoords.Item2] = 0;
        }
    
        private (int, int) GetRandomEmptyPosition(Member member)
        {
            var dimX = member.Matrix.GetLength(0);
            var dimY = member.Matrix.GetLength(1);

            while (true)
            {
                var randX = Random.Next(dimX);
                var randY = Random.Next(dimY);
                if (member[randX, randY] == 0)
                {
                    return (randX, randY);
                }
            }
        }
    }
}
