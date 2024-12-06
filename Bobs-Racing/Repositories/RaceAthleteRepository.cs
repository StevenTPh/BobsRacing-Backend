using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Repositories
{
    public class RaceAthleteRepository : IRaceAthleteRepository
    {
        private readonly AppDbContext _context;

        public RaceAthleteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveRaceResultsAsync(List<RaceAthlete> raceResults)
        {
            _context.RaceAthletes.AddRange(raceResults);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RaceAthlete>> GetAllRaceAthleteAsync()
        {
            return await _context.RaceAthletes.ToListAsync();
        }

        public async Task<RaceAthlete> GetRaceAthleteByIdAsync(int id)
        {
            return await _context.RaceAthletes
                         .FirstOrDefaultAsync(ra => ra.RaceAthleteId == id);
        }

        public async Task AddRaceAthleteAsync(RaceAthlete raceAthlete)
        {
            _context.RaceAthletes.Add(raceAthlete);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRaceAthleteAsync(RaceAthlete raceAthlete)
        {
            _context.RaceAthletes.Update(raceAthlete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRaceAthleteAsync(int id)
        {

            var raceAthlete = await _context.RaceAthletes.FindAsync(id);
            if (raceAthlete != null)
            {
                _context.RaceAthletes.Remove(raceAthlete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidateAthleteAsync(int athleteId)
        {
            return await _context.Athletes.AnyAsync(a => a.AthleteId == athleteId);
        }

        public async Task<bool> ValidateRaceAsync(int raceId)
        {
            return await _context.Races.AnyAsync(r => r.RaceId == raceId);
        }
    }
}
