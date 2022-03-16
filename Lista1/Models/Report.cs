namespace Lista1.Models
{
    public class Report
    {
        public TimeSpan Time { get; set; }
        public int PopulationSize { get; set; }
        public int SubPopulationSize { get; set; }
        public int Rounds { get; set; }
        public double CrossChance { get; set; }
        public double EliteSize { get; set; }
        public int MaxTournamentChampions { get; set; }
        public int DimX { get; set; }
        public int DimY { get; set; }
        public int MachinesCount { get; set; }
        public List<RoundStats> RoundStats { get; set; }
        public Member BestMember { get; set; }
        public string SelectionOperator { get; set; }
    }
}
