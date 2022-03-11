using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface ISelectionOperator
    {
        public List<Member> Select(int count, List<Member> source, int currentRound);
    }
}
