using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBets()
        {
            var bets = await _betRepository.GetAllBetsAsync();
            return Ok(bets);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBetById(int id)
        {
            var bet = await _betRepository.GetBetByIdAsync(id);
            if (bet == null)
            {
                return NotFound("Bet not found");
            }

            // Get user role and ID from token
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userRole == "User" && int.TryParse(userIdClaim, out var userId) && bet.UserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to access this bet.");
            }
            return Ok(bet);
        }

        [Authorize(Roles = "User")]
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
    }
}
