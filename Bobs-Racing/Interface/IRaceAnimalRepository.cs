using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceAnimalRepository
    {
        Task<IEnumerable<RaceAnimal>> GetAllRaceAnimalAsync();
        Task<RaceAnimal> GetBetByIdAsync(int animalId, int raceId);
        Task AddRaceAnimalAsync(RaceAnimal raceAnimal);
        Task UpdateRaceAnimalAsync(RaceAnimal raceAnimal);
        Task DeleteRaceAnimalAsync(int animalId, int raceId);

        // New method to validate the composite key in RaceAnimal
        Task<bool> ValidateAnimalAsync(int animalId);
        Task<bool> ValidateRaceAsync(int raceId);

        Task SaveRaceResultsAsync(List<RaceAnimal> raceResults);
    }
}
