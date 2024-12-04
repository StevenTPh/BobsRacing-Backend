using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var animals = await _animalRepository.GetAllAnimalsAsync();
            return Ok(animals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalById(int id)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound("Animal not found");
            }

            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _animalRepository.AddAnimalAsync(animal);

            return CreatedAtAction(nameof(GetAnimalById), new { id = animal.AnimalId }, animal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] Animal animal)
        {
            var existingAnimal = await _animalRepository.GetAnimalByIdAsync(id);
            if (existingAnimal == null)
            {
                return NotFound("Animal not found");
            }

            existingAnimal.Image = animal.Image;
            existingAnimal.Name = animal.Name;
            existingAnimal.MinSpeed = animal.MinSpeed;
            existingAnimal.MaxSpeed = animal.MaxSpeed;

            await _animalRepository.UpdateAnimalAsync(existingAnimal);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound("Animal not found");
            }

            await _animalRepository.DeleteAnimalAsync(id);
            return NoContent();
        }
    }
}
