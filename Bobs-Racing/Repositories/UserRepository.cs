using Bobs_Racing.Data;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Racing.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Bets) // Include related Bets
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Bets) // Include related Bets
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddUserAsync(User user)
        {
            // Hash the password before storing
            //user.Password = HashPassword(user.Password);

            //user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /*
        public async Task UpdateUserCredentialsAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                // Update password only if it's different
                if (!string.IsNullOrEmpty(user.Password) &&
                    !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
                {
                    existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
                // Update only fields that are provided
                if (!string.IsNullOrEmpty(user.Profilename))
                {
                    existingUser.Profilename = user.Profilename;
                }

                if (!string.IsNullOrEmpty(user.Username))
                {
                    existingUser.Username = user.Username;
                }

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateUserCreditsAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                existingUser.Credits = user.Credits;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        } */
        public async Task UpdateUserAsync(int userId, UserDTO userDto)
        {
            var existingUser = await _context.Users.FindAsync(userId);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(userDto.Profilename))
            {
                existingUser.Profilename = userDto.Profilename;
            }

            if (!string.IsNullOrEmpty(userDto.Username))
            {
                existingUser.Username = userDto.Username;
            }

            if (!string.IsNullOrEmpty(userDto.Password) &&
                !BCrypt.Net.BCrypt.Verify(userDto.Password, existingUser.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            if (!string.IsNullOrEmpty(userDto.Role))
            {
                existingUser.Role = userDto.Role;
            }

            // Always update credits if provided
            if (userDto.Credits != 0)
            {
                existingUser.Credits = userDto.Credits;
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }




        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

/*        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Name == username);
        }*/

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
