using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetRepository _betRepository;
        private IUserRepository _userRepository;

        public BetController(IBetRepository betRepository, IUserRepository userRepository)
        {
            _betRepository = betRepository;
            _userRepository = userRepository;
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

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] Bet bet)
        {
                   /*
                {
                      "amount": 0, (User input for amount)
                      "potentialPayout": calculated in frontend,
                      "userId": 0,
                      "raceAthleteId": 0
                    }*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            // Validate the composite key
            if (!await _betRepository.ValidateRaceAthleteAsync(bet.RaceAthleteId))
            {
                return BadRequest("Invalid RaceId or AthleteId combination");
            }

            var user = await _userRepository.GetUserByIdAsync(bet.UserId);

            if (user.Credits < bet.Amount)
            {
                return BadRequest("not enough credits for this bet");
            }

            user.Credits -= (double)bet.Amount;

            var userDto = new UserDTO
            {
                Credits = user.Credits,
                Profilename = user.Profilename,
                Username = user.Username,
                Role = user.Role,
            };

            await _userRepository.UpdateUserAsync(user.UserId, userDto);

            await _betRepository.AddBetAsync(bet);
            return CreatedAtAction(nameof(GetBetById), new { id = bet.BetId }, bet);
        }

    }
}
