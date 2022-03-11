namespace Lista1.Models
{
    public class Report
    {
        public int PopulationSize { get; set; }
        public int SubPopulationSize { get; set; }
        public int Rounds { get; set; }
        public double CrossWish { get; set; }
        public double EliteSize { get; set; }
        public int MaxTournamentChampions { get; set; }
        public int DimX { get; set; }
        public int DimY { get; set; }
        public int MachinesCount { get; set; }
        public List<RoundStats> RoundStats { get; set; }
        public Member BestMember { get; set; }
    }
}
