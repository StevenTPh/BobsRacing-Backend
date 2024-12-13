using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Services;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaceController : ControllerBase
    {
        private readonly IRaceRepository _raceRepository;
        private readonly OddsCalculatorService _oddsCalculatorService;

        public RaceController(IRaceRepository raceRepository, OddsCalculatorService oddsCalculatorService)
        {
            _raceRepository = raceRepository;
            _oddsCalculatorService = oddsCalculatorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRaces()
        {
            var races = await _raceRepository.GetAllRacesAsync();
            // Map to RaceDTO
            var raceDTOs = races.Select(race => new RaceDTO
            {
                RaceId = race.RaceId,
                Date = race.Date,
                RaceAthletes = race.RaceAthletes
            }).ToList();

            return Ok(raceDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaceById(int id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null)
            {
                return NotFound("Race not found");
            }
            // Map to RaceDTO
            var raceDTO = new RaceDTO
            {
                RaceId = race.RaceId,
                Date = race.Date,
                RaceAthletes = race.RaceAthletes
            };

            return Ok(raceDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRace([FromBody] Race race)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _raceRepository.AddRaceAsync(race);

            return CreatedAtAction(nameof(GetRaceById), new { id = race.RaceId }, race);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRace(int id, [FromBody] Race race)
        {
            var existingRace = await _raceRepository.GetRaceByIdAsync(id);
            if (existingRace == null)
            {
                return NotFound("Race not found");
            }

            existingRace.Date = race.Date;


            // Optionally handle sensitive updates like password hashing
            await _raceRepository.UpdateRaceAsync(existingRace);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
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

        [HttpGet("{id}/odds")]
        public async Task<IActionResult> GetRaceOdds(int id)
        {
            // Fetch the race with its athletes
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null || race.RaceAthletes == null)
            {
                return NotFound("Race not found or no athletes in this race.");
            }

            var athletes = race.RaceAthletes.Select(ra => ra.Athlete).ToList();
            if (athletes == null || athletes.Count == 0)
            {
                return BadRequest("No valid athlete data found for this race.");
            }

            // Calculate odds using the updated service method
            var odds = _oddsCalculatorService.CalculateOddsBasedOnBestAverage(race.RaceAthletes, athletes);

            // Format response with athleteId included
            var result = race.RaceAthletes.Select(ra => new
            {
                RaceAthleteId = ra.RaceAthleteId,
                AthleteId = odds[ra.RaceAthleteId].AthleteId,
                AthleteName = ra.Athlete?.Name,
                Odds = odds[ra.RaceAthleteId].Odds
            });

            return Ok(result);
        }

    }
}
