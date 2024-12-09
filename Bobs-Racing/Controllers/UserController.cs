using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;
using Bobs_Racing.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            return Ok(user);
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
                Profilename = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User", // Automatically assign the "User" role
                Credits = 0 // Set default credits
            };

            await _userRepository.AddUserAsync(user);

            return CreatedAtAction(nameof(Register), new { id = user.UserId }, new { user.UserId, user.Profilename, user.Role });
        }


        [Authorize(Roles = "User")]
        [HttpPut("{id}/credentials")]
        public async Task<IActionResult> UpdateUserCredentials(int id, [FromBody] User user)
        {

            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId) || userId != id)
            {
                return Forbid("You are not allowed to update another user's credentials.");
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            existingUser.Profilename = user.Profilename;
            existingUser.Password = user.Password;

            // Optionally handle sensitive updates like password hashing
            await _userRepository.UpdateUserCredentialsAsync(existingUser);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/credits")]
        public async Task<IActionResult> UpdateUserCredits(int id, [FromBody] User user)
        {
/*            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch");
            }*/

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            existingUser.Credits = user.Credits;

            // Optionally handle sensitive updates like password hashing
            await _userRepository.UpdateUserCreditsAsync(existingUser);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] string role)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Role = role;
            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userRole == "User" && (!int.TryParse(userIdClaim, out var userId) || userId != id))
            {
                return Forbid("You are not allowed to delete another user.");
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [Authorize]
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


            var token = _tokenGenerator.GenerateToken(user.UserId, user.Profilename, user.Role);


            return Ok(new { Token = token });
        }

    }
}
