using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Operators
{
    public class NoMutation : IMutationOperator
    {
        public string Name => nameof(NoMutation);

        public bool CanRegister(int dimX, int dimY, int machinesCount) => true;

        public void Mutate(Member member){}
    }
}
