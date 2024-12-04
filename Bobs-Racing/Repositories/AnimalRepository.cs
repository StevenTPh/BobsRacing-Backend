using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;


namespace Bobs_Racing.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly AppDbContext _context;

        public AnimalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAnimalsByIdsAsync(List<int> animalIds)
        {
            return await _context.Animals
                .Where(a => animalIds.Contains(a.AnimalId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            return await _context.Animals
                .Include(a => a.RaceAnimals)
                    .ThenInclude(ra => ra.Race)
                .ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            return await _context.Animals
                .Include(a => a.RaceAnimals)
                    .ThenInclude(ra => ra.Race)
                .FirstOrDefaultAsync(a => a.AnimalId == id);
        }

        public async Task AddAnimalAsync(Animal animal)
        {
            await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnimalAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();
            }
        }

    }
}
