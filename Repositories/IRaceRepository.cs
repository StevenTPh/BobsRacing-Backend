using System.Diagnostics;
using Bobs_Racing.Model;

namespace Bobs_Racing.Repositories
{
    public interface IRaceRepository
    {
        Task<Race> GetRaceByIdAsync(int raceId, string result);
        Task CreateRaceAsync(Race race);
    }

}
