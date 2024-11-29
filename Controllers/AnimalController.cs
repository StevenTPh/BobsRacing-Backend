using Bobs_Racing.Data;
using Bobs_Racing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimalController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var animals = await _context.Animals
                .Include(a => a.RaceAnimals)
                    .ThenInclude(ra => ra.Race)
                .ToListAsync();

            return Ok(animals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalById(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.RaceAnimals)
                    .ThenInclude(ra => ra.Race)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal == null)
                return NotFound();

            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimalById), new { id = animal.AnimalId }, animal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] Animal animal)
        {
            if (id != animal.AnimalId)
                return BadRequest("Animal ID mismatch.");

            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
                return NotFound();

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
