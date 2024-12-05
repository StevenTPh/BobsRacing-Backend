using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceService
    {
        // a model for processing the race logic similar across all uses of this interface
        Task<List<RaceAnimal>> ProcessRaceAsync(int raceId, List<Animal> animals);
    }
}
