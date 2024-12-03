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
            // Validate the composite key in RaceAnimal
            if (!await ValidateRaceAnimalAsync(bet.RaceId, bet.AnimalId))
            {
                throw new ArgumentException("Invalid RaceId or AnimalId combination.");
            }

            await _context.Bets.AddAsync(bet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBetAsync(Bet bet)
        {
            // Validate the composite key in RaceAnimal
            if (!await ValidateRaceAnimalAsync(bet.RaceId, bet.AnimalId))
            {
                throw new ArgumentException("Invalid RaceId or AnimalId combination.");
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

        public async Task<bool> ValidateRaceAnimalAsync(int raceId, int animalId)
        {
            return await _context.RaceAnimals.AnyAsync(ra => ra.RaceId == raceId && ra.AnimalId == animalId);
        }
    }
}
