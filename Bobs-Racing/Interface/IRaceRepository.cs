using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllRacesAsync();
        Task<Race> GetRaceByIdAsync(int id);
        Task AddRaceAsync(Race race);
        Task UpdateRaceAsync(Race race);
        Task DeleteRaceAsync(int id);

        Task UpdateRaceIsFinishedAsync(int raceId, bool isFinished);

    }
}
