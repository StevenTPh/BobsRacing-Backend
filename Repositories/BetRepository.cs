using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly AppDbContext _context;

        public BetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bet>> GetAllBetsAsync()
        {
            return await _context.Bets
                .Include(b => b.Race)
                .Include(b => b.Animal)
                .ToListAsync();
        }

        public async Task<Bet> GetBetByIdAsync(int betId)
        {
            return await _context.Bets
                .Include(b => b.Race)
                .Include(b => b.Animal)
                .FirstOrDefaultAsync(b => b.BetID == betId);
        }

        public async Task CreateBetAsync(Bet bet)
        {
            await _context.Bets.AddAsync(bet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBetAsync(Bet bet)
        {
            _context.Bets.Update(bet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBetAsync(int betId)
        {
            var bet = await _context.Bets.FindAsync(betId);
            if (bet != null)
            {
                _context.Bets.Remove(bet);
                await _context.SaveChangesAsync();
            }
        }
    }

}
