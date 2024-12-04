using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Repositories
{
    public class RaceAnimalRepository : IRaceAnimalRepository
    {
        private readonly AppDbContext _context;

        public RaceAnimalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RaceAnimal>> GetAllRaceAnimalAsync()
        {
            return await _context.RaceAnimals.ToListAsync();
        }

        public async Task<RaceAnimal> GetRaceAnimalByIdAsync(int id)
        {
            return await _context.RaceAnimals
                .Include(ra => ra.RaceAnimalId)
                .FirstOrDefaultAsync(ra => ra.RaceAnimalId == id);
        }

        public async Task AddRaceAnimalAsync(RaceAnimal raceAnimal)
        {
            _context.RaceAnimals.Add(raceAnimal);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRaceAnimalAsync(RaceAnimal raceAnimal)
        {
            _context.RaceAnimals.Update(raceAnimal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRaceAnimalAsync(int id)
        {

            var raceAnimal = await _context.RaceAnimals.FindAsync(id);
            if (raceAnimal != null)
            {
                _context.RaceAnimals.Remove(raceAnimal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidateAnimalAsync(int animalId)
        {
            return await _context.Animals.AnyAsync(a => a.AnimalId == animalId);
        }

        public async Task<bool> ValidateRaceAsync(int raceId)
        {
            return await _context.Races.AnyAsync(r => r.RaceId == raceId);
        }
    }
}
