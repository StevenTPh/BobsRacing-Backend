using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                return NotFound();
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

            await _betRepository.CreateBetAsync(bet);
            return CreatedAtAction(nameof(GetBetById), new { id = bet.BetID }, bet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBet(int id, [FromBody] Bet bet)
        {
            if (id != bet.BetID)
            {
                return BadRequest("Bet ID mismatch.");
            }

            await _betRepository.UpdateBetAsync(bet);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBet(int id)
        {
            await _betRepository.DeleteBetAsync(id);
            return NoContent();
        }
    }

}
