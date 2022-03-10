using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface ICrossoverOperator
    {
        int ChildrenSize { get; }
        Member[] Cross(Member first, Member second);
    }
}
