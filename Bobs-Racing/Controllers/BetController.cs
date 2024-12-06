using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetRepository _betRepository;

        public BetController(IBetRepository betRepository)
        {
            _betRepository = betRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBets()
        {
            var bets = await _betRepository.GetAllBetsAsync();
            return Ok(bets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBetById(int id)
        {
            var bet = await _betRepository.GetBetByIdAsync(id);
            if (bet == null)
            {
                return NotFound("Bet not found");
            }
            return Ok(bet);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] Bet bet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate the composite key
            if (!await _betRepository.ValidateRaceAthleteAsync(bet.RaceAthlete.RaceAthleteId))
            {
                return BadRequest("Invalid RaceId or AthleteId combination");
            }

            await _betRepository.AddBetAsync(bet);
            return CreatedAtAction(nameof(GetBetById), new { id = bet.BetId }, bet);
        }

/*        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBet(int id, [FromBody] Bet bet)
        {
            // Validate the composite key
            if (!await _betRepository.ValidateRaceAnimalAsync(bet.RaceId, bet.AnimalId))
            {
                return BadRequest("Invalid RaceId or AnimalId combination");
            }

            await _betRepository.UpdateBetAsync(bet);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBet(int id)
        {
            await _betRepository.DeleteBetAsync(id);
            return NoContent();
        }*/
    }
}
