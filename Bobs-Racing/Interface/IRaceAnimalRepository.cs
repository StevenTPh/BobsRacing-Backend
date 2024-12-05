using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceAnimalRepository
    {
        Task<IEnumerable<RaceAnimal>> GetAllRaceAnimalAsync();
        Task<RaceAnimal> GetRaceAnimalByIdAsync(int id);
        Task AddRaceAnimalAsync(RaceAnimal raceAnimal);
        Task UpdateRaceAnimalAsync(RaceAnimal raceAnimal);
        Task DeleteRaceAnimalAsync(int id);

        // New method to validate the composite key in RaceAnimal
        Task<bool> ValidateAnimalAsync(int animalId);
        Task<bool> ValidateRaceAsync(int raceId);

        Task SaveRaceResultsAsync(List<RaceAnimal> raceResults);
    }
}
