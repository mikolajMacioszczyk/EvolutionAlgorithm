using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Managers
{
    public class MutationManager : IMutationManager
    {
        private static Random _random = new Random();
        private List<(IMutationOperator, int, int)> mutationOperators = new List<(IMutationOperator, int, int)>();

        private int summChance;

        public IEnumerable<(string, double)> GetOparatorsInfo()
        {
            return mutationOperators
                .Select(kv => (kv.Item1.Name, 1.0 * kv.Item3 / summChance * 100))
                .OrderByDescending(kv => kv.Item2);
        }

        public void MutatePopulation(List<Member> members)
        {
            foreach (var member in members)
            {
                var choice = _random.Next(summChance);
                var mutationOperator = mutationOperators.FirstOrDefault(m => m.Item2 > choice).Item1;
                if (mutationOperator != null)
                {
                    mutationOperator.Mutate(member);
                }
            }
        }

        public void RegisterOperator(IMutationOperator mutationOperator, int chance, int dimX, int dimY, int machinesCount)
        {
            if (mutationOperator.CanRegister(dimX, dimY, machinesCount))
            {
                summChance += chance;
                mutationOperators.Add((mutationOperator, summChance, chance));
            }
            else
            {
                Console.WriteLine($"Cannot register {mutationOperator.Name} with dimX: {dimX}, dimY: {dimY}, machinesCount: {machinesCount}");
            }
        }
    }
}
