using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface IReproductionOperator
    {
        public List<Member> GenerateChildren(List<Member> parents, int count);
    }
}
