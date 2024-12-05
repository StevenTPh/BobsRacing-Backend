using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceService
    {
        Task<List<RaceAnimal>> ProcessRaceAsync(int raceId, List<Animal> animals);
    }
}
