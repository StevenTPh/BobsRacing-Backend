using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Controllers;
using Bobs_Racing.Models;
using Bobs_Racing.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bobs_Racing.Tests.Controllers
{

    public class RaceControllerTests
    {
        private readonly AppDbContext _context;
        private readonly RaceController _controller;

        public RaceControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRaceDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new RaceController(_context);

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
        public async Task GetAllRaces_ShouldReturnOkResult()
        {
            // Act
            var result = await _controller.GetAllRaces();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var races = Assert.IsType<List<Race>>(okResult.Value);
            Assert.Single(races);
        }

        [Fact]
        public async Task GetRaceById_ShouldReturnOkResult_WhenRaceExists()
        {
            // Act
            var result = await _controller.GetRaceById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var race = Assert.IsType<Race>(okResult.Value);
            Assert.Equal(1, race.RaceId);
        }

        [Fact]
        public async Task GetRaceById_ShouldReturnNotFound_WhenRaceDoesNotExist()
        {
            // Act
            var result = await _controller.GetRaceById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateRace_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newRace = new Race { RaceId = 2, Date = DateTime.Now.AddDays(1) };

            // Act
            var result = await _controller.CreateRace(newRace);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdRace = Assert.IsType<Race>(createdResult.Value);
            Assert.Equal(2, createdRace.RaceId);
        }

        [Fact]
        public async Task UpdateRace_ShouldReturnNoContent_WhenRaceExists()
        {
            // Arrange
            var updatedRace = new Race { RaceId = 1, Date = DateTime.Now.AddDays(2) };

            // Act
            var result = await _controller.UpdateRace(1, updatedRace);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var race = await _context.Races.FindAsync(1);
            Assert.Equal(updatedRace.Date, race.Date);
        }

        [Fact]
        public async Task DeleteRace_ShouldReturnNoContent_WhenRaceExists()
        {
            // Act
            var result = await _controller.DeleteRace(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var race = await _context.Races.FindAsync(1);
            Assert.Null(race);
        }

        [Fact]
        public async Task DeleteRace_ShouldReturnNotFound_WhenRaceDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteRace(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }


}
