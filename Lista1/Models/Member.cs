namespace Lista1.Models
{
    public class Member
    {
        // TODO: Przetrzymuj inaczej
        public int[,] Matrix { get; set; }

        public Member(int dimX, int dimY)
        {
            Matrix = new int[dimX, dimY];
        }

        public Member DeepCopy()
        {
            var copy = new Member(Matrix.GetLength(0), Matrix.GetLength(1));
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    copy[i, j] = Matrix[i, j];
                }
            }
            return copy;
        }

        public int this[int i, int j]{
            get { return Matrix[i, j]; }
            set { Matrix[i, j] = value; }
        }

        public (int, int) GetCoordinatesOfNumber(int number)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j] == number)
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
    }
}
