using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAllBetsAsync();
        Task<Bet> GetBetByIdAsync(int betId);
        Task CreateBetAsync(Bet bet);
        Task UpdateBetAsync(Bet bet);
        Task DeleteBetAsync(int betId);
    }
}


