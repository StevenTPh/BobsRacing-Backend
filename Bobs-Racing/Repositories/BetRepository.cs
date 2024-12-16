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
                .Include(b => b.RaceAthlete)
                    .ThenInclude(ra => ra.Race)
                .Include(b => b.RaceAthlete)
                    .ThenInclude(ra => ra.Athlete)
                .ToListAsync();
        }

        public async Task<Bet> GetBetByIdAsync(int betId)
        {
            return await _context.Bets
                .Include(b => b.User)
                .Include(b => b.RaceAthlete)
                    .ThenInclude(ra => ra.Race)
                .Include(b => b.RaceAthlete)
                    .ThenInclude(ra => ra.Athlete)
                .FirstOrDefaultAsync(b => b.BetId == betId);
        }

        public async Task AddBetAsync(Bet bet)
        {
            // Validate if the RaceAnimalId exists
            if (!await ValidateRaceAthleteAsync(bet.RaceAthleteId))
            {
                throw new ArgumentException("Invalid RaceAthleteId.");
            }

            await _context.Bets.AddAsync(bet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBetAsync(Bet bet)
        {
            // Validate if the RaceAnimalId exists
            if (!await ValidateRaceAthleteAsync(bet.RaceAthleteId))
            {
                throw new ArgumentException("Invalid RaceAthleteId.");
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
        public async Task<bool> ValidateRaceAthleteAsync(int raceAthleteId)
        {
            // Check if a RaceAnimal with the given raceAnimalId exists in the database
            return await _context.RaceAthletes.AnyAsync(ra => ra.RaceAthleteId == raceAthleteId);
        }
    }
}
