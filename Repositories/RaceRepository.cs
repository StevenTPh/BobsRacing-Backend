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
                .Include(r => r.Animals)
                .FirstOrDefaultAsync(r => r.RaceId == raceId);
        }

        public async Task<IEnumerable<Race>> GetAllRacesAsync()
        {
            return await _context.Races
                .Include(r => r.Animals)
                .ToListAsync();
        }

        public async Task CreateRaceAsync(Race race)
        {
            await _context.Races.AddAsync(race);
            await _context.SaveChangesAsync();
        }
        public async Task AddAnimalToRace(int raceId, int animalId)
        {
            var race = await _context.Races.Include(r => r.Animals).FirstOrDefaultAsync(r => r.RaceId == raceId);
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.AnimalId == animalId);

            if (race != null && animal != null && !race.Animals.Contains(animal))
            {
                // join table
                race.Animals.Add(animal);
                await _context.SaveChangesAsync();
            }
        }
    }


}
