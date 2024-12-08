﻿using Bobs_Racing.Data;
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

        public async Task UpdateUserCredentialsAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserCreditsAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                // Example: Only update specific fields
                //existingUser.Name = user.Name;
                existingUser.Credits = user.Credits;
               
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                // Update properties as needed
                existingUser.Profilename = user.Profilename;
                existingUser.Password = user.Password; // Ensure password is hashed before calling this
                existingUser.Role = user.Role;
                existingUser.Credits = user.Credits;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
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
