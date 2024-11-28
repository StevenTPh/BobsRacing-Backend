using Bobs_Racing.Model;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAllAnimals()
        {
            var animals = _animalRepository.GetAll();
            return Ok(animals);
        }
        [HttpGet("{id}")]
        public ActionResult<Animal> GetAnimalById(int id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }
        [HttpPost]
        public async Task<ActionResult> AddAnimal([FromBody] Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _animalRepository.Add(animal);
            await _animalRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnimal(int id, [FromBody] Animal updatedAnimal)
        {
            if (!ModelState.IsValid || id != updatedAnimal.Id)
            {
                return BadRequest(ModelState);
            }
            var existingAnimal = _animalRepository.GetById(id);
            if (existingAnimal == null)
            {
                return BadRequest(ModelState);
            }
            _animalRepository.Update(updatedAnimal);
            await _animalRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimal(int id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
            {
                return BadRequest(ModelState);
            }
            _animalRepository.Delete(animal);
            await _animalRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
