using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Models;

namespace Bobs_Racing.Interface
{
    public interface IAnimalRepository
    {
        Task<IEnumerable<Animal>> GetAllAsync();
        Task<Animal> GetByIdAsync(int id);
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(int id);
    }
}
