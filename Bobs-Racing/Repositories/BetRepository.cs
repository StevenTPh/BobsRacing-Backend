using Bobs_Racing.Data;
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
                .Include(b => b.User)
                .Include(b => b.RaceAnimal)
                    .ThenInclude(ra => ra.Race)
                .Include(b => b.RaceAnimal)
                    .ThenInclude(ra => ra.Animal)
                .ToListAsync();
        }

        public async Task<Bet> GetBetByIdAsync(int betId)
        {
            return await _context.Bets
                .Include(b => b.User)
                .Include(b => b.RaceAnimal)
                    .ThenInclude(ra => ra.Race)
                .Include(b => b.RaceAnimal)
                    .ThenInclude(ra => ra.Animal)
                .FirstOrDefaultAsync(b => b.BetId == betId);
        }

        public async Task AddBetAsync(Bet bet)
        {
            // Validate if the RaceAnimalId exists
            if (!await ValidateRaceAnimalAsync(bet.RaceAnimal.RaceAnimalId))
            {
                throw new ArgumentException("Invalid RaceAnimalId.");
            }

            await _context.Bets.AddAsync(bet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBetAsync(Bet bet)
        {
            // Validate if the RaceAnimalId exists
            if (!await ValidateRaceAnimalAsync(bet.RaceAnimal.RaceAnimalId))
            {
                throw new ArgumentException("Invalid RaceAnimalId.");
            }

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

        // This method now only takes raceAnimalId as a parameter
        public async Task<bool> ValidateRaceAnimalAsync(int raceAnimalId)
        {
            // Check if a RaceAnimal with the given raceAnimalId exists in the database
            return await _context.RaceAnimals.AnyAsync(ra => ra.RaceAnimalId == raceAnimalId);
        }
    }
}
