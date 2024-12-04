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
        private readonly AppDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUserRepositoryDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);

            // Reset the database for each test run
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
            _context.Users.Add(new User { UserId = 1, Name = "User1", Password = "Password1", Credits = 100 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Act
            var users = await _repository.GetAllUsersAsync();

            // Assert
            Assert.Single(users);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
        {
            // Act
            var user = await _repository.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.UserId);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // Act
            var user = await _repository.GetUserByIdAsync(99);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUser()
        {
            // Arrange
            var newUser = new User { UserId = 2, Name = "User2", Password = "Password2", Credits = 200 };

            // Act
            await _repository.AddUserAsync(newUser);

            // Assert
            var user = await _context.Users.FindAsync(2);
            Assert.NotNull(user);
            Assert.Equal("User2", user.Name);
        }

        [Fact]
        public async Task UpdateUserCredentialsAsync_ShouldUpdateUserCredentials()
        {
            // Arrange
            var updatedUser = new User { UserId = 1, Name = "UpdatedUser", Password = "UpdatedPassword" };

            // Act
            await _repository.UpdateUserCredentialsAsync(updatedUser);

            // Assert
            var user = await _context.Users.FindAsync(1);
            Assert.Equal("UpdatedUser", user.Name);
        }

        [Fact]
        public async Task UpdateUserCreditsAsync_ShouldUpdateUserCredits()
        {
            // Arrange
            var updatedUser = new User { UserId = 1, Credits = 500 };

            // Act
            await _repository.UpdateUserCreditsAsync(updatedUser);

            // Assert
            var user = await _context.Users.FindAsync(1);
            Assert.Equal(500, user.Credits);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUser_WhenExists()
        {
            // Act
            await _repository.DeleteUserAsync(1);

            // Assert
            var user = await _context.Users.FindAsync(1);
            Assert.Null(user);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDoNothing_WhenDoesNotExist()
        {
            // Act
            await _repository.DeleteUserAsync(99);

            // Assert
            var user = await _context.Users.FindAsync(99);
            Assert.Null(user);
        }
    }

}
