using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Models.Input;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceAthleteController : ControllerBase
    {
        private readonly IRaceAthleteRepository _raceAthleteRepository;
        private readonly IRaceService _raceService;
        private readonly IAthleteRepository _athleteRepository;

        public RaceAthleteController(IRaceAthleteRepository raceAthleteRepository,
                                    IRaceService raceService,
                                    IAthleteRepository athleteRepository)
        {
            _raceAthleteRepository = raceAthleteRepository;
            _raceService = raceService;
            _athleteRepository = athleteRepository;
        }

        [HttpPost("process-race")]
        public async Task<IActionResult> ProcessRace([FromBody] RaceAthleteInputModel raceInput)
        {
            if (raceInput.AthleteIds == null || !raceInput.AthleteIds.Any())
                return BadRequest("Athlete IDs cannot be empty.");

            // Fetch all animals in list based on id
            var athletes = await _athleteRepository.GetAthletesByIdsAsync(raceInput.AthleteIds);

            if (!athletes.Any())
                return NotFound("No athletes found for the given IDs.");

            // Process the race
            var raceResults = await _raceService.ProcessRaceAsync(raceInput.RaceId, athletes);
            return Ok(raceResults);
        }

        // GET: api/RaceAthlete
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceAthlete>>> GetAllRaceAthletes()
        {
            var raceAthletes = await _raceAthleteRepository.GetAllRaceAthleteAsync();
            if (raceAthletes == null)
            {
                return NotFound();
            }
            return Ok(raceAthletes);
        }

        // GET: api/RaceAthlete/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RaceAthlete>> GetRaceAthleteById(int id)
        {
            var raceAthlete = await _raceAthleteRepository.GetRaceAthleteByIdAsync(id);
            if (raceAthlete == null)
            {
                return NotFound();
            }
            return Ok(raceAthlete);
        }

        // POST: api/RaceAthlete
        [HttpPost]
        public async Task<ActionResult> AddRaceAthlete([FromBody] RaceAthlete raceAthlete)
        {
            if (raceAthlete == null)
            {
                return BadRequest("Invalid data.");
            }

            var isValidAthlete = await _raceAthleteRepository.ValidateAthleteAsync(raceAthlete.AthleteId);
            var isValidRace = await _raceAthleteRepository.ValidateRaceAsync(raceAthlete.RaceId);

            if (!isValidAthlete)
            {
                return BadRequest($"Athlete with ID {raceAthlete.AthleteId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceAthlete.RaceId} is not valid.");
            }

            await _raceAthleteRepository.AddRaceAthleteAsync(raceAthlete);
            return CreatedAtAction(nameof(GetRaceAthleteById), new { id = raceAthlete.RaceAthleteId }, raceAthlete);
        }

        // PUT: api/RaceAthlete/{id}
        [HttpPut ("{id}")]
        public async Task<ActionResult> UpdateRaceAthlete(int id, [FromBody] RaceAthlete raceAthlete)
        {
            var exisitngRaceAthlete = await _raceAthleteRepository.GetRaceAthleteByIdAsync(id);

            var isValidAthlete = await _raceAthleteRepository.ValidateAthleteAsync(raceAthlete.AthleteId);
            var isValidRace = await _raceAthleteRepository.ValidateRaceAsync(raceAthlete.RaceId);

            if (!isValidAthlete)
            {
                return BadRequest($"Athlete with ID {raceAthlete.AthleteId} is not valid.");
            }

            if (!isValidRace)
            {
                return BadRequest($"Race with ID {raceAthlete.Race.RaceId} is not valid.");
            }

            exisitngRaceAthlete.RaceId = raceAthlete.RaceId;
            exisitngRaceAthlete.AthleteId= raceAthlete.AthleteId;

            await _raceAthleteRepository.UpdateRaceAthleteAsync(exisitngRaceAthlete);
            return NoContent();
        }

        // DELETE: api/RaceAthlete/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRaceAthlete(int id)
        {

            var raceAthlete = await _raceAthleteRepository.GetRaceAthleteByIdAsync(id);
            if (raceAthlete == null)
            {
                return NotFound("RaceAthlete not found");
            }

            await _raceAthleteRepository.DeleteRaceAthleteAsync(id);
            return NoContent();
        }
    }
}
