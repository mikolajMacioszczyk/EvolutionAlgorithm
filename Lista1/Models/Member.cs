using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista1.Models
{
    public class Member
    {
        public int[,] Matrix { get; set; }

        public Member(int dimX, int dimY)
        {
            Matrix = new int[dimX, dimY];
        }

        public int this[int i, int j]{
            get { return Matrix[i, j]; }
            set { Matrix[i, j] = value; }
        }
    }
}
