using Bobs_Racing.Model;
using Bobs_Racing.Data;
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

        public IEnumerable<Animal> GetAll()
        {
            return _context.Animals.ToList();
        }

        public Animal GetById(int id)
        {
            return _context.Animals.FirstOrDefault(a => a.AnimalId == id);
        }

        public void Add(Animal animal)
        {
            _context.Animals.Add(animal);
        }
        public void Update(Animal animal)
        { 
            _context.Animals.Update(animal);
        }
        public void Delete(Animal animal)
        {
            _context.Animals.Remove(animal);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
