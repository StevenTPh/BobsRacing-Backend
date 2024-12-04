using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bobs_Racing.Controllers;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.EntityFrameworkCore;
using Bobs_Racing.Controllers;
using Bobs_Racing.Data;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bobs_Racing.Tests.Controllers
{

    public class UserControllerTests
    {
        private readonly AppDbContext _context;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUserDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new UserController(new UserRepository(_context));

            // Reset the database for each test run
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
            _context.Users.Add(new User { UserId = 1, Name = "User1", Password = "Password1", Credits = 100 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOkResult()
        {
            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsType<List<User>>(okResult.Value);
            Assert.Single(users);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOkResult_WhenUserExists()
        {
            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.Equal(1, user.UserId);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Act
            var result = await _controller.GetUser(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newUser = new User { UserId = 2, Name = "User2", Password = "Password2", Credits = 200 };

            // Act
            var result = await _controller.CreateUser(newUser);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdUser = Assert.IsType<User>(createdResult.Value);
            Assert.Equal(2, createdUser.UserId);
        }

        [Fact]
        public async Task UpdateUserCredentials_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var updatedUser = new User { UserId = 1, Name = "UpdatedUser", Password = "UpdatedPassword" };

            // Act
            var result = await _controller.UpdateUserCredentials(1, updatedUser);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var user = await _context.Users.FindAsync(1);
            Assert.Equal("UpdatedUser", user.Name);
        }

        [Fact]
        public async Task UpdateUserCredits_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var updatedUser = new User { UserId = 1, Credits = 500 };

            // Act
            var result = await _controller.UpdateUserCredits(1, updatedUser);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var user = await _context.Users.FindAsync(1);
            Assert.Equal(500, user.Credits);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserExists()
        {
            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var user = await _context.Users.FindAsync(1);
            Assert.Null(user);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteUser(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }


}
