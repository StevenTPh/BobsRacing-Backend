using Bobs_Racing.Model;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpGet("{id}/{result}")]
        public async Task<IActionResult> GetRaceById(int id, string result)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id, result);
            if (race == null)
            {
                return NotFound();
            }
            return Ok(race);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRace([FromBody] Race race)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _raceRepository.CreateRaceAsync(race);
            return CreatedAtAction(nameof(GetRaceById), new { id = race.RaceId, result = race.Result }, race);
        }
    }

}
