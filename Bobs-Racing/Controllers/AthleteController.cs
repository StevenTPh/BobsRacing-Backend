using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AthleteController : ControllerBase
    {
        private readonly IAthleteRepository _athleteRepository;

        public AthleteController(IAthleteRepository athleteRepository)
        {
            _athleteRepository = athleteRepository;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAthletes()
        {
            var athletes = await _athleteRepository.GetAllAthletesAsync();
            return Ok(athletes);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAthleteById(int id)
        {
            var athlete = await _athleteRepository.GetAthleteByIdAsync(id);
            if (athlete == null)
            {
                return NotFound("Athlete not found");
            }

            return Ok(athlete);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAthlete([FromBody] Athlete athlete)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _athleteRepository.AddAthleteAsync(athlete);

            return CreatedAtAction(nameof(GetAthleteById), new { id = athlete.AthleteId }, athlete);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAthlete(int id, [FromBody] Athlete athlete)
        {
            var existingAthlete = await _athleteRepository.GetAthleteByIdAsync(id);
            if (existingAthlete == null)
            {
                return NotFound("Athlete not found");
            }

            existingAthlete.Image = athlete.Image;
            existingAthlete.Name = athlete.Name;
            existingAthlete.SlowestTime = athlete.SlowestTime;
            existingAthlete.FastestTime = athlete.FastestTime;

            await _athleteRepository.UpdateAthleteAsync(existingAthlete);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAthlete(int id)
        {
            var athlete = await _athleteRepository.GetAthleteByIdAsync(id);
            if (athlete == null)
            {
                return NotFound("Athlete not found");
            }

            await _athleteRepository.DeleteAthleteAsync(id);
            return NoContent();
        }
    }
}
