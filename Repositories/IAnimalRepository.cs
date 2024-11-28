using Bobs_Racing.Model;

namespace Bobs_Racing.Repositories
{
    public interface IAnimalRepository
    {
        IEnumerable<Animal> GetAll();
        Animal? GetById(int id);
        void Add(Animal animal);
        void Update(Animal animal);
        void Delete(Animal animal);
        Task SaveChangesAsync();
    }
}
