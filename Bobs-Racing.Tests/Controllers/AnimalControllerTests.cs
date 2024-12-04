using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Controllers;
using Bobs_Racing.Models;
using Bobs_Racing.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Bobs_Racing.Data;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Tests.Controllers
{

    public class AnimalControllerTests
    {
        private readonly AppDbContext _context;
        private readonly AnimalController _controller;

        public AnimalControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAnimalDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new AnimalController(_context);

            // Seed data
            _context.Animals.Add(new Animal { AnimalId = 1, Name = "Animal1", MinSpeed = 10, MaxSpeed = 20 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAnimals_ShouldReturnOkResult()
        {
            // Act
            var result = await _controller.GetAllAnimals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var animals = Assert.IsType<List<Animal>>(okResult.Value);
            Assert.Single(animals);
        }

        [Fact]
        public async Task GetAnimalById_ShouldReturnOkResult_WhenAnimalExists()
        {
            // Act
            var result = await _controller.GetAnimalById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var animal = Assert.IsType<Animal>(okResult.Value);
            Assert.Equal(1, animal.AnimalId);
            Assert.Equal("Animal1", animal.Name);
        }

        [Fact]
        public async Task GetAnimalById_ShouldReturnNotFound_WhenAnimalDoesNotExist()
        {
            // Act
            var result = await _controller.GetAnimalById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateAnimal_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newAnimal = new Animal { AnimalId = 2, Name = "Animal2", MinSpeed = 15, MaxSpeed = 25 };

            // Act
            var result = await _controller.CreateAnimal(newAnimal);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdAnimal = Assert.IsType<Animal>(createdResult.Value);
            Assert.Equal("Animal2", createdAnimal.Name);
        }

        [Fact]
        public async Task UpdateAnimal_ShouldReturnNoContent_WhenAnimalExists()
        {
            // Arrange
            var updatedAnimal = new Animal { AnimalId = 1, Name = "UpdatedAnimal1", MinSpeed = 12, MaxSpeed = 22 };

            // Act
            var result = await _controller.UpdateAnimal(1, updatedAnimal);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var animal = await _context.Animals.FindAsync(1);
            Assert.Equal("UpdatedAnimal1", animal.Name);
        }

        [Fact]
        public async Task DeleteAnimal_ShouldReturnNoContent_WhenAnimalExists()
        {
            // Act
            var result = await _controller.DeleteAnimal(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var animal = await _context.Animals.FindAsync(1);
            Assert.Null(animal);
        }

        [Fact]
        public async Task DeleteAnimal_ShouldReturnNotFound_WhenAnimalDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteAnimal(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }


}
