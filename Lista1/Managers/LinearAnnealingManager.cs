using Lista1.Interfaces;

namespace Lista1.Managers
{
    public class LinearAnnealingManager : IAnnealingManager
    {
        private readonly int _rounds;
        private readonly int _maxChapions;

        public LinearAnnealingManager(int rounds, int maxChapions)
        {
            _rounds = rounds;
            _maxChapions = maxChapions;
        }

        public int GetChampionsCount(int currentRound)
        {
            return (int) Math.Ceiling(_maxChapions * (1.0 * (currentRound + 1) / _rounds));
        }
    }
}
