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
    public class RaceController : ControllerBase
    {
        private readonly IRaceRepository _raceRepository;

        public RaceController(IRaceRepository raceRepository)
        {
            _raceRepository = raceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRaces()
        {
            var races = await _raceRepository.GetAllRacesAsync();
            return Ok(races);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaceById(int id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null)
            {
                return NotFound("Race not found");
            }
            return Ok(race);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRace([FromBody] Race race)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _raceRepository.AddRaceAsync(race);

            return CreatedAtAction(nameof(GetRaceById), new { id = race.RaceId }, race);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRace(int id, [FromBody] Race race)
        {
            var existingRace = await _raceRepository.GetRaceByIdAsync(id);
            if (existingRace == null)
            {
                return NotFound("Race not found");
            }

            existingRace.Date = race.Date;
            //existingRace.RaceAnimals = race.RaceAnimals;


            // Optionally handle sensitive updates like password hashing
            await _raceRepository.UpdateRaceAsync(existingRace);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null)
            {
                return NotFound("Race not found");
            }

            await _raceRepository.DeleteRaceAsync(id);
            return NoContent();
        }
    }
}
