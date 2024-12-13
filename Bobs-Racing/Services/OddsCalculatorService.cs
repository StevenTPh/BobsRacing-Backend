using Bobs_Racing.Models;

namespace Bobs_Racing.Services;

public class OddsCalculatorService
{
    public Dictionary<int, (int AthleteId, double Odds)> CalculateOddsBasedOnBestAverage(
        List<RaceAthlete> raceAthletes,
        List<Athlete> athletes,
        double baseOdds = 1.5,
        double scaleFactor = 50)
    {
        // Step 1: Calculate average time for each athlete
        var athleteAverages = raceAthletes.ToDictionary(
            ra => ra.RaceAthleteId,
            ra =>
            {
                var athlete = athletes.FirstOrDefault(a => a.AthleteId == ra.AthleteId);
                return athlete != null
                    ? (athlete.FastestTime + athlete.SlowestTime) / 2
                    : double.MaxValue;
            });

        // Step 2: Find the best (lowest) average time
        double bestAverage = athleteAverages.Values.Min();

        // Step 3: Calculate odds based on difference from best average
        return athleteAverages.ToDictionary(
            pair => pair.Key,
            pair =>
            {
                var athleteId = raceAthletes.First(ra => ra.RaceAthleteId == pair.Key).AthleteId;
                double difference = pair.Value - bestAverage;
                double odds = baseOdds + (difference * scaleFactor);
                return (athleteId, Math.Round(odds, 2));
            });
    }
}
