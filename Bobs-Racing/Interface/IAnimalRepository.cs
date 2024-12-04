using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAnimalsByIdsAsync(List<int> animalIds);
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalByIdAsync(int id);
        Task AddAnimalAsync(Animal animal);
        Task UpdateAnimalAsync(Animal animal);
        Task DeleteAnimalAsync(int id);
    }
}
