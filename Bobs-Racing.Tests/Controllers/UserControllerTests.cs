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

namespace Bobs_Racing.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _controller = new UserController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
        {
            new User { UserId = 1, Name = "John", Credits = 100 },
            new User { UserId = 2, Name = "Jane", Credits = 200 }
        };
            _mockRepo.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOkResult_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John", Credits = 100 };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal("John", returnedUser.Name);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John", Password = "password" };
            _mockRepo.Setup(repo => repo.AddUserAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateUser(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedUser = Assert.IsType<User>(createdResult.Value);
            Assert.Equal("John", returnedUser.Name);
        }

        [Fact]
        public async Task UpdateUserCredentials_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "John", Password = "newpassword" };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.UpdateUserCredentialsAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUserCredentials(1, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1 };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.DeleteUserAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }

}
