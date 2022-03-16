using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface IMutationOperator
    {
        public string Name { get; }
        public bool CanRegister(int dimX, int dimY, int machinesCount);
        public void Mutate(Member member);
    }
}
