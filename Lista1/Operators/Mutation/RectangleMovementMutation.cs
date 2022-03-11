using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators.Mutation
{
    public class RectangleMovementMutation : IMutationOperator
    {
        private static Random random = new Random();
        
        private readonly int _dimX;
        
        private readonly int _dimY;

        public RectangleMovementMutation(int dimX, int dimY)
        {
            _dimX = dimX;
            _dimY = dimY;
        }

        public void Mutate(Member member)
        {
            // wybierz rozmiary prostokąta
            var xLength = random.Next(1, _dimX - 1);
            var yLength = random.Next(1, _dimY - 1);

            // wybierz położenie lewego rogu (tak żeby się mieścił na planszy)
            var topOffset = random.Next(0, _dimX - xLength);
            var leftOffset = random.Next(0, _dimY - yLength);

            switch (random.Next(0, 4))
            {
                case 0:
                    MoveTop(member, xLength, yLength, topOffset, leftOffset);
                    break;
                case 1:
                    // w prawo
                    break;
                case 2:
                    // w dół
                    break;
                case 3:
                    // w lewo
                    break;
            }
        }

        private void MoveTop(Member member, int xLength, int yLength, int topOffset, int leftOffset)
        {
            if (topOffset == 0) 
            { 
                return; 
            }

            var move = random.Next(1, topOffset);

            for (int i = leftOffset; i < leftOffset + yLength; i++)
            {
                for (int j = 0; j < xLength; j++)
                {

                }
            }
        }
    }
}
