using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllAsync();
        Task<Race> GetByIdAsync(int id);
        Task AddAsync(Race race);
        Task UpdateAsync(Race race);
        Task DeleteAsync(int id);
    }
}
