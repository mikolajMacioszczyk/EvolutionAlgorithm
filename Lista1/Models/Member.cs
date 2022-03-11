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

        // only for test purpose - not efficient
        public bool IsValid(int maxValue)
        {
            for (int i = 1; i < maxValue; i++)
            {
                var x = GetCoordinatesOfNumber(i);
                if (x.Item1 < 0 || x.Item1 >= Matrix.GetLength(0) || x.Item2 < 0 || x.Item2 >= Matrix.GetLength(1))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
