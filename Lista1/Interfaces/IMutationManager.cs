using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface IMutationManager
    {
        public void MutatePopulation(List<Member> members);
        public void RegisterOperator(IMutationOperator mutationOperator, int chance);
    }
}
