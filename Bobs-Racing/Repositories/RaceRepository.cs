using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bobs_Racing.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly AppDbContext _context;

        public RaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Race>> GetAllRacesAsync()
        {
            return await _context.Races
                .Include(r => r.RaceAthletes)
                    .ThenInclude(ra => ra.Athlete)
                .ToListAsync();
        }

        public async Task<Race> GetRaceByIdAsync(int id)
        {
            return await _context.Races
                .Include(r => r.RaceAthletes)
                    .ThenInclude(ra => ra.Athlete)
                .FirstOrDefaultAsync(r => r.RaceId == id);
        }

        public async Task AddRaceAsync(Race race)
        {
            await _context.Races.AddAsync(race);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRaceAsync(Race race)
        {
            _context.Races.Update(race);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRaceAsync(int id)
        {
            var race = await _context.Races.FindAsync(id);
            if (race != null)
            {
                _context.Races.Remove(race);
                await _context.SaveChangesAsync();
            }
        }

    }
}
