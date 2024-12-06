using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;


namespace Bobs_Racing.Repositories
{
    public class AthleteRepository : IAthleteRepository
    {
        private readonly AppDbContext _context;

        public AthleteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Athlete>> GetAthletesByIdsAsync(List<int> athleteIds)
        {
            return await _context.Athletes
                .Where(a => athleteIds.Contains(a.AthleteId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Athlete>> GetAllAthletesAsync()
        {
            return await _context.Athletes
                .Include(a => a.RaceAthletes)
                    .ThenInclude(ra => ra.Race)
                .ToListAsync();
        }

        public async Task<Athlete> GetAthleteByIdAsync(int id)
        {
            return await _context.Athletes
                .Include(a => a.RaceAthletes)
                    .ThenInclude(ra => ra.Race)
                .FirstOrDefaultAsync(a => a.AthleteId == id);
        }

        public async Task AddAthleteAsync(Athlete athlete)
        {
            await _context.Athletes.AddAsync(athlete);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAthleteAsync(Athlete athlete)
        {
            _context.Athletes.Update(athlete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAthleteAsync(int id)
        {
            var animal = await _context.Athletes.FindAsync(id);
            if (animal != null)
            {
                _context.Athletes.Remove(animal);
                await _context.SaveChangesAsync();
            }
        }

    }
}
