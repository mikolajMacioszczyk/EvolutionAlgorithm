using Lista1.Interfaces;
using Lista1.Models;

namespace Lista1.Managers
{
    public class MutationManager : IMutationManager
    {
        private static Random _random = new Random();
        private List<(IMutationOperator, int)> mutationOperators = new List<(IMutationOperator, int)>();

        private int summChance;

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

        public void RegisterOperator(IMutationOperator mutationOperator, int chance)
        {
            summChance += chance;
            mutationOperators.Add((mutationOperator, summChance));
        }
    }
}
