using Bobs_Racing.Models;

namespace Bobs_Racing.Services;
public class OddsCalculatorService
{
    private const double HouseEdge = 0.1;

    public double CalculatePerformanceScore(double fastestTime, double slowestTime, double weightFastest = 0.9, double weightSlowest = 0.5)
    {
        return (fastestTime * weightFastest) + (slowestTime * weightSlowest);
    }

    public Dictionary<int, double> CalculateProbabilities(List<RaceAthlete> raceAthletes, List<Athlete> athletes)
    {
        var performanceScores = raceAthletes.ToDictionary(
            ra => ra.RaceAthleteId,
            ra =>
            {
                var athlete = athletes.FirstOrDefault(a => a.AthleteId == ra.AthleteId);
                return athlete != null ? CalculatePerformanceScore(athlete.FastestTime, athlete.SlowestTime) : double.MaxValue;
            });

        double totalInverseScore = performanceScores.Values.Sum(score => 1 / score);

        return performanceScores.ToDictionary(
            pair => pair.Key,
            pair => (1 / pair.Value) / totalInverseScore
        );
    }

    public double CalculateOdds(double probability)
    {
        return Math.Round(1 / (probability * (1 - HouseEdge)), 2); // Round to 2 decimal places
    }
}
