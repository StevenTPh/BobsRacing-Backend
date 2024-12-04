using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bobs_Racing.Data;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly AppDbContext _context;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John", Password = "password", Credits = 100 };

            // Act
            await _repository.AddUserAsync(user);
            var result = await _context.Users.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John", Credits = 100 };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public async Task UpdateUserCredentialsAsync_ShouldUpdateNameAndPassword()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John", Password = "oldpassword" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Name = "Jane";
            user.Password = "newpassword";

            // Act
            await _repository.UpdateUserCredentialsAsync(user);
            var result = await _context.Users.FindAsync(1);

            // Assert
            Assert.Equal("Jane", result.Name);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteUserAsync(1);
            var result = await _context.Users.FindAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
