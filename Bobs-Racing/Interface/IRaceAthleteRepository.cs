using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceAthleteRepository
    {
        Task<IEnumerable<RaceAthlete>> GetAllRaceAthleteAsync();
        Task<RaceAthlete> GetRaceAthleteByIdAsync(int id);

        Task<List<RaceAthlete>> GetAthletesByIdsAsyncList(List<int> raceAthleteIds);
        Task AddRaceAthleteAsync(RaceAthlete raceAthlete);
        Task UpdateRaceAthleteAsync(RaceAthlete raceAthlete);
        Task UpdateRaceAthleteFinalPositionAsync(int raceAthleteId, int finalPosition);
        Task DeleteRaceAthleteAsync(int id);

        // New method to validate the composite key in RaceAnimal
        Task<bool> ValidateAthleteAsync(int athleteId);
        Task<bool> ValidateRaceAsync(int raceId);

        Task SaveRaceResultsAsync(List<RaceAthlete> raceResults);
    }
}
