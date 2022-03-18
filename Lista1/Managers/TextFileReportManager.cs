using Lista1.Interfaces;
using Lista1.Models;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Lista1.Managers
{
    internal class TextFileReportManager : IReportManager
    {
        public void Save(Report report)
        {
            string directory = GetOutputDirectory();

            var path = Path.Combine(directory, $"{DateTime.Now.ToShortDateString()}_{Guid.NewGuid()}.txt");
            SaveReport(path, report);
            Console.WriteLine($"Report saved to {path}");
        }

        private void SaveReport(string path, Report report)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            var sb = new StringBuilder($"MaxhinesCount: {report.MachinesCount}\n" +
                $"DimX: {report.DimX}\n" +
                $"DimY: {report.DimY}\n" +
                $"Learning parameters: \n" +
                $"Max rounds without progress: {report.MaxRoundsWithoutProgress}\n" +
                $"Population size: {report.PopulationSize}\n" +
                $"Subpopulation size: {report.SubPopulationSize}\n" +
                $"Elite size: {report.EliteSize}\n" +
                $"Cross probability: {report.CrossProbability}\n" +
                $"Max tournament champions: {report.MaxTournamentChampions}\n" +
                $"\n" +
                $"Selection operator: {report.SelectionOperator}\n" +
                $"Evaluation operator: {report.EvaluationOperator}\n" +
                $"Mustation operators:\n");

            foreach (var mutationInfo in report.MutationOperatorsInfo)
            {
                sb.Append($" - {mutationInfo.Item1} with {mutationInfo.Item2.ToString("0.##")}% chance\n");
            }

            sb.Append($"\n" +
                $"Time: {report.Time.Milliseconds} ms\n" +
                $"\n" +
                $"Best result: \n" +
                $"{JsonSerializer.Serialize(report.BestMember.ToJaggedMatrix())}\n" +
                $"\n" +
                $"Round Statistics: (csv)\n" +
                $"Round,Best,Worst,Average\n");

            for (int i = 0; i < report.RoundStats.Count; i++)
            {
                var stats = report.RoundStats[i];
                sb.Append($"{i+1},{stats.Best},{stats.Worst},{stats.Average.ToString("0.00", CultureInfo.InvariantCulture)}\n");
            }

            File.WriteAllText(path, sb.ToString());
        }

        private string GetOutputDirectory()
        {
            var root = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
            var directoryPath = Path.Combine(root, "Output");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }
    }
}
