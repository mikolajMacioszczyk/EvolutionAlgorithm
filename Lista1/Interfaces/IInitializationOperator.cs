using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface IInitializationOperator
    {
        List<Member> InitializePopulation(int populationSize, int dimX, int dimY, int machinesCount);
    }
}
