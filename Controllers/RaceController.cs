using Bobs_Racing.Data;
using Bobs_Racing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RaceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRaces()
        {
            var races = await _context.Races
                .Include(r => r.RaceAnimals)
                    .ThenInclude(ra => ra.Animal)
                .ToListAsync();

            return Ok(races);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaceById(int id)
        {
            var race = await _context.Races
                .Include(r => r.RaceAnimals)
                    .ThenInclude(ra => ra.Animal)
                .FirstOrDefaultAsync(r => r.RaceId == id);

            if (race == null)
                return NotFound();

            return Ok(race);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRace([FromBody] Race race)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Races.AddAsync(race);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRaceById), new { id = race.RaceId }, race);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRace(int id, [FromBody] Race race)
        {
            if (id != race.RaceId)
                return BadRequest("Race ID mismatch.");

            _context.Races.Update(race);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _context.Races.FindAsync(id);
            if (race == null)
                return NotFound();

            _context.Races.Remove(race);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
