using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bobs_Racing.Data;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bobs_Racing.Tests.Repositories
{

    public class RaceRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly RaceRepository _repository;

        public RaceRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRaceRepositoryDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new RaceRepository(_context);

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
        public async Task GetAllAsync_ShouldReturnAllRaces()
        {
            // Act
            var races = await _repository.GetAllAsync();

            // Assert
            Assert.Single(races);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRace_WhenExists()
        {
            // Act
            var race = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(race);
            Assert.Equal(1, race.RaceId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // Act
            var race = await _repository.GetByIdAsync(99);

            // Assert
            Assert.Null(race);
        }

        [Fact]
        public async Task AddAsync_ShouldAddRace()
        {
            // Arrange
            var newRace = new Race { RaceId = 2, Date = DateTime.Now.AddDays(1) };

            // Act
            await _repository.AddAsync(newRace);

            // Assert
            var race = await _context.Races.FindAsync(2);
            Assert.NotNull(race);
            Assert.Equal(2, race.RaceId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRace()
        {
            // Arrange
            var updatedRace = new Race { RaceId = 1, Date = DateTime.Now.AddDays(2) };

            // Act
            await _repository.UpdateAsync(updatedRace);

            // Assert
            var race = await _context.Races.FindAsync(1);
            Assert.Equal(updatedRace.Date, race.Date);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRace_WhenExists()
        {
            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var race = await _context.Races.FindAsync(1);
            Assert.Null(race);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDoNothing_WhenDoesNotExist()
        {
            // Act
            await _repository.DeleteAsync(99);

            // Assert
            var race = await _context.Races.FindAsync(99);
            Assert.Null(race);
        }
    }


}
