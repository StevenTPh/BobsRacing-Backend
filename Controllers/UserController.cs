using Microsoft.AspNetCore.Mvc;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;

namespace Bobs_Racing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
/*
            if (await _userRepository.IsUsernameTakenAsync(user.Name))
            {
                return BadRequest("Username is already taken");
            }*/

            // Hash the password before storing it (ensure hashing in repository)
            await _userRepository.AddUserAsync(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch");
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            // Optionally handle sensitive updates like password hashing
            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
