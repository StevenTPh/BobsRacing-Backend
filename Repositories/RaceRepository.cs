using System;
using System.Diagnostics;
using Bobs_Racing.Data;
using Bobs_Racing.Model;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly AppDbContext _context;

        public RaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Race> GetRaceByIdAsync(int raceId, string result)
        {
            return await _context.Races
                .Include(r => r.Animals) // Include related Animals
                .FirstOrDefaultAsync(r => r.RaceId == raceId && r.Result == result);
        }

        public async Task CreateRaceAsync(Race race)
        {
            await _context.Races.AddAsync(race);
            await _context.SaveChangesAsync();
        }
    }


}
