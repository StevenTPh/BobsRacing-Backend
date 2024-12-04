using Bobs_Racing.Models;

namespace Bobs_Racing.Services
{
    public interface IRaceService
    {
        Task<List<RaceAnimal>> ProcessRaceAsync(List<RaceAnimal> starlist);
    }
}
