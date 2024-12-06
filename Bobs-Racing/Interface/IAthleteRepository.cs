using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IAthleteRepository
    {
        Task<List<Athlete>> GetAthletesByIdsAsync(List<int> athleteIds);
        Task<IEnumerable<Athlete>> GetAllAthletesAsync();
        Task<Athlete> GetAthleteByIdAsync(int id);
        Task AddAthleteAsync(Athlete athlete);
        Task UpdateAthleteAsync(Athlete athlete);
        Task DeleteAthleteAsync(int id);
    }
}
