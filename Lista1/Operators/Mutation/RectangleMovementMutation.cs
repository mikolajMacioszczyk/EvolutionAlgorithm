using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
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
            // choose rectangle dimensions
            var xLength = random.Next(1, _dimX - 1);
            var yLength = random.Next(1, _dimY - 1);

            // choose left corner coordinates
            var topOffset = random.Next(0, _dimX - xLength + 1);
            var leftOffset = random.Next(0, _dimY - yLength + 1);

            switch (random.Next( 4))
            {
                case 0:
                    MoveTop(member, xLength, yLength, topOffset, leftOffset);
                    break;
                case 1:
                    MoveDown(member, xLength, yLength, topOffset, leftOffset);
                    break;
                case 2:
                    MoveRight(member, xLength, yLength, topOffset, leftOffset);
                    break;
                case 3:
                    MoveLeft(member, xLength, yLength, topOffset, leftOffset);
                    break;
            }
        }

        private void MoveTop(Member member, int xLength, int yLength, int topOffset, int leftOffset)
        {
            if (topOffset == 0) 
            { 
                return; 
            }

            int move = random.Next(topOffset) + 1;

            var tempRow = new int[move, yLength];
            for (int j = 0; j < move; j++)
            {
                for (int i = 0; i < yLength; i++)
                {
                    tempRow[j, i] = member[topOffset - move + j, leftOffset + i];
                }
            }

            for (int i = topOffset; i < topOffset + xLength; i++)
            {
                for (int j = leftOffset; j < leftOffset + yLength; j++)
                {
                    member[i - move, j] = member[i, j];
                }
            }

            for (int j = 0; j < move; j++)
            {
                for (int i = 0; i < yLength; i++)
                {
                    member[topOffset - move + j + xLength, leftOffset + i] = tempRow[j, i];
                }
            }
        }

        private void MoveDown(Member member, int xLength, int yLength, int topOffset, int leftOffset)
        {
            int nextRow = topOffset + xLength;
            if (nextRow >= _dimX)
            {
                return;
            }

            int move = random.Next(_dimX - nextRow) + 1;

            var tempRow = new int[yLength];
            for (int i = 0; i < yLength; i++)
            {
                tempRow[i] = member[nextRow, leftOffset + i];
            }

            for (int i = nextRow; i > topOffset; i--)
            {
                for (int j = leftOffset; j < leftOffset + yLength; j++)
                {
                    member[i, j] = member[i - 1, j];
                }
            }

            for (int i = 0; i < yLength; i++)
            {
                member[topOffset, leftOffset + i] = tempRow[i];
            }
        }

        private void MoveRight(Member member, int xLength, int yLength, int topOffset, int leftOffset)
        {
            if (leftOffset == 0)
            {
                return;
            }

            int move = random.Next(leftOffset) + 1;

            var tempRow = new int[move, xLength];
            for (int j = 0; j < move; j++)
            {
                for (int i = 0; i < xLength; i++)
                {
                    tempRow[j, i] = member[topOffset + i, leftOffset - move + j];
                }
            }

            for (int i = topOffset; i < topOffset + xLength; i++)
            {
                for (int j = leftOffset; j < leftOffset + yLength; j++)
                {
                    member[i, j - move] = member[i, j];
                }
            }

            for (int j = 0; j < move; j++)
            {
                for (int i = 0; i < xLength; i++)
                {
                    member[topOffset + i, leftOffset - move + j + yLength] = tempRow[j, i];
                }
            }
        }

        private void MoveLeft(Member member, int xLength, int yLength, int topOffset, int leftOffset)
        {
            int nextCol = leftOffset + yLength;
            if (nextCol >= _dimY)
            {
                return;
            }

            int move = random.Next(_dimY - nextCol) + 1;

            var tempRow = new int[xLength];
            for (int i = 0; i < xLength; i++)
            {
                tempRow[i] = member[topOffset + i, nextCol];
            }

            for (int i = topOffset; i < topOffset + xLength; i++)
            {
                for (int j = nextCol; j > leftOffset; j--)
                {
                    member[i, j] = member[i, j - 1];
                }
            }

            for (int i = 0; i < xLength; i++)
            {
                member[topOffset + i, leftOffset] = tempRow[i];
            }
        }
    }
}
