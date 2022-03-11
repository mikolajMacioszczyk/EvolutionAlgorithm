using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class CascadeCrossoverOperator : ICrossoverOperator
    {
        private static Random Random = new Random();
        public int ChildrenSize => 2;
        
        private double CrossWish { get; set; }

        public int MaxNumber { get; set; }

        public CascadeCrossoverOperator(int maxNumber, double crossWish)
        {
            MaxNumber = maxNumber;
            CrossWish = crossWish;
        }

        public Member[] Cross(Member first, Member second)
        {
            var child1 = first.DeepCopy();
            var child2 = second.DeepCopy();

            var number = Random.Next(0, MaxNumber) + 1;

            PerformCross(number, child1, second);
            PerformCross(number, child2, first);

            return new Member[] { child1, child2 };
        }

        private void PerformCross(int number, Member active, Member passive)
        {
            var coords = active.GetCoordinatesOfNumber(number);
            var exchangedNumber = passive[coords.Item1, coords.Item2];
            
            // no change
            if (number == exchangedNumber)
            {
                return;
            }

            // empty field
            if (exchangedNumber == 0)
            {
                SwapWithEmptyField(number, active, passive, coords);
            }
            else
            {
                SwapWithValues(number, exchangedNumber, active, passive, coords);
            }
        }

        private void SwapWithValues(int number, int exchangedNumber, Member active, Member passive, (int, int) coords)
        {
            // swap values
            var newCoords = active.GetCoordinatesOfNumber(exchangedNumber);
            active[coords.Item1, coords.Item2] = exchangedNumber;
            active[newCoords.Item1, newCoords.Item2] = number;

            if (Random.NextDouble() < CrossWish)
            {
                // recurence with the same values
                // bubbles starting values
                PerformCross(number, active, passive);
            }
        }

        private void SwapWithEmptyField(int number, Member active, Member passive, (int, int) coords)
        {
            while (number > 0)
            {
                active[coords.Item1, coords.Item2] = 0;

                // TODO: bug
                if (Random.NextDouble() < CrossWish && false)
                {
                    // exchange based on structure of passive Member

                    // get number position on passive's grid
                    coords = passive.GetCoordinatesOfNumber(number);
                    // get value on that position
                    var temp = active[coords.Item1, coords.Item2];

                    // move step foreward
                    active[coords.Item1, coords.Item2] = number;
                    number = temp;
                }
                else
                {
                    // find random zero-value position
                    var zeroCoords = GetRandomEmptyPosition(active);
                    // swap with that position
                    active[zeroCoords.Item1, zeroCoords.Item2] = number;
                    number = 0;
                }
            }
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
