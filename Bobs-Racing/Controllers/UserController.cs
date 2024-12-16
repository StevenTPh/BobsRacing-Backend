using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;
using Bobs_Racing.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenGenerator _tokenGenerator;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenGenerator = new JwtTokenGenerator(_configuration);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Get user role and ID from token
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userRole == "User" && int.TryParse(userIdClaim, out var userId) && user.UserId != userId)
            {
                return Forbid("You are not allowed to access this user.");
            }

            return Ok(new { user.UserId, user.Profilename, user.Username, user.Credits, user.Role });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("bets")]
        public async Task<IActionResult> GetUserWithBets(int id)
        {
            var userBets = await _userRepository.GetUserWithBetsAsync(id);

            if (userBets == null)
            {
                return NotFound("User not found.");
            }


            return Ok(userBets);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("{id}/credentials")]
        public async Task<IActionResult> UpdateUserCredentials(int id, [FromBody] UserDTO user)
        {
            var userIdClaim = HttpContext.User.FindFirst("id")?.Value;
            var userRoleClaim = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(userIdClaim, out var userId) || (userId != id && userRoleClaim != "Admin"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to update another user's credentials.");
            }
            try
            {
                await _userRepository.UpdateUserAsync(id, user);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{id}/credits")]
        public async Task<IActionResult> UpdateUserCredits(int id, [FromBody] UserDTO user)
        {

            var userIdClaim = HttpContext.User.FindFirst("id")?.Value;
            var userRoleClaim = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(userIdClaim, out var userId) || (userId != id && userRoleClaim != "Admin"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to update another user's credits.");
            }

            try
            {
                await _userRepository.UpdateUserAsync(id, new UserDTO { Credits = user.Credits });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UserDTO user)
        {
            try
            {
                await _userRepository.UpdateUserAsync(id, new UserDTO { Role = user.Role });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userIdClaim = HttpContext.User.FindFirst("id")?.Value;
            var userRoleClaim = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(userIdClaim, out var userId) || (userId != id && userRoleClaim != "Admin"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to delete another user's credits.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already taken");
            }

            var user = new User
            {
                Username = request.Username,
                Profilename = request.Profilename,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User", // Automatically assign the "User" role
                Credits = 0 // Set default credits
            };

            await _userRepository.AddUserAsync(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, new { user.UserId, user.Username, user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _tokenGenerator.GenerateToken(user.UserId, user.Username, user.Profilename, user.Role, user.Credits);

            return Ok(new { Token = token });
        }
    }
}
