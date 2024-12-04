using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Controllers;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
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

    public class RaceAnimalControllerTests
    {
        private readonly AppDbContext _context;
        private readonly RaceAnimalController _controller;

        public RaceAnimalControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRaceAnimalDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new RaceAnimalController(new RaceAnimalRepository(_context));

            // Reset the database for each test run
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
            var race = new Race { RaceId = 1, Date = DateTime.Now };
            var animal = new Animal { AnimalId = 1, Name = "Animal1", Image = "animal.jpg", MinSpeed = 10, MaxSpeed = 20 };

            _context.Races.Add(race);
            _context.Animals.Add(animal);
            _context.RaceAnimals.Add(new RaceAnimal { RaceId = 1, AnimalId = 1, CheckpointSpeeds = new int[] { 10, 15, 20 }, FinalPosition = 1 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllRaceAnimals_ShouldReturnOkResult()
        {
            // Act
            var result = await _controller.GetAllRaceAnimals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var raceAnimals = Assert.IsType<List<RaceAnimal>>(okResult.Value);
            Assert.Single(raceAnimals);
        }

        [Fact]
        public async Task GetBetById_ShouldReturnOkResult_WhenRaceAnimalExists()
        {
            // Act
            var result = await _controller.GetBetById(1, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var raceAnimal = Assert.IsType<RaceAnimal>(okResult.Value);
            Assert.Equal(1, raceAnimal.RaceId);
            Assert.Equal(1, raceAnimal.AnimalId);
        }

        [Fact]
        public async Task GetBetById_ShouldReturnNotFound_WhenRaceAnimalDoesNotExist()
        {
            // Act
            var result = await _controller.GetBetById(99, 99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddRaceAnimal_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newRaceAnimal = new RaceAnimal { RaceId = 1, AnimalId = 2, CheckpointSpeeds = new int[] { 12, 18, 25 }, FinalPosition = 2 };
            _context.Animals.Add(new Animal { AnimalId = 2, Name = "Animal2", Image = "animal.jpg", MinSpeed = 15, MaxSpeed = 25 });
            _context.SaveChanges();

            // Act
            var result = await _controller.AddRaceAnimal(newRaceAnimal);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdRaceAnimal = Assert.IsType<RaceAnimal>(createdResult.Value);
            Assert.Equal(2, createdRaceAnimal.AnimalId);
        }

        [Fact]
        public async Task UpdateRaceAnimal_ShouldReturnNoContent_WhenRaceAnimalExists()
        {
            // Arrange
            var updatedRaceAnimal = new RaceAnimal { RaceId = 1, AnimalId = 1, CheckpointSpeeds = new int[] { 14, 20, 30 }, FinalPosition = 1 };

            // Act
            var result = await _controller.UpdateRaceAnimal(updatedRaceAnimal);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var raceAnimal = await _context.RaceAnimals.FindAsync(1, 1);
            Assert.Equal(30, raceAnimal.CheckpointSpeeds[2]);
        }

        [Fact]
        public async Task DeleteRaceAnimal_ShouldReturnNoContent_WhenRaceAnimalExists()
        {
            // Act
            var result = await _controller.DeleteRaceAnimal(1, 1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var raceAnimal = await _context.RaceAnimals.FindAsync(1, 1);
            Assert.Null(raceAnimal);
        }

        [Fact]
        public async Task DeleteRaceAnimal_ShouldReturnNotFound_WhenRaceAnimalDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteRaceAnimal(99, 99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }


}
