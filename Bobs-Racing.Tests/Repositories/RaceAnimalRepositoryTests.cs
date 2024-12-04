using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class RaceAnimalRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly RaceAnimalRepository _repository;

        public RaceAnimalRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRaceAnimalRepositoryDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new RaceAnimalRepository(_context);

            // Seed data
            var race = new Race { RaceId = 1, Date = DateTime.Now };
            var animal = new Animal { AnimalId = 1, Name = "Animal1", MinSpeed = 10, MaxSpeed = 20 };

            _context.Races.Add(race);
            _context.Animals.Add(animal);
            _context.RaceAnimals.Add(new RaceAnimal { RaceId = 1, AnimalId = 1, CheckpointSpeeds = new int[] { 10, 15, 20 }, FinalPosition = 1 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllRaceAnimalAsync_ShouldReturnAllRaceAnimals()
        {
            // Act
            var raceAnimals = await _repository.GetAllRaceAnimalAsync();

            // Assert
            Assert.Single(raceAnimals);
        }

        [Fact]
        public async Task GetBetByIdAsync_ShouldReturnRaceAnimal_WhenExists()
        {
            // Act
            var raceAnimal = await _repository.GetBetByIdAsync(1, 1);

            // Assert
            Assert.NotNull(raceAnimal);
            Assert.Equal(1, raceAnimal.RaceId);
            Assert.Equal(1, raceAnimal.AnimalId);
        }

        [Fact]
        public async Task GetBetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // Act
            var raceAnimal = await _repository.GetBetByIdAsync(99, 99);

            // Assert
            Assert.Null(raceAnimal);
        }

        [Fact]
        public async Task AddRaceAnimalAsync_ShouldAddRaceAnimal()
        {
            // Arrange
            var newRaceAnimal = new RaceAnimal { RaceId = 1, AnimalId = 2, CheckpointSpeeds = new int[] { 12, 18, 25 }, FinalPosition = 2 };
            _context.Animals.Add(new Animal { AnimalId = 2, Name = "Animal2", MinSpeed = 15, MaxSpeed = 25 });
            _context.SaveChanges();

            // Act
            await _repository.AddRaceAnimalAsync(newRaceAnimal);

            // Assert
            var raceAnimal = await _context.RaceAnimals.FindAsync(1, 2);
            Assert.NotNull(raceAnimal);
            Assert.Equal(25, raceAnimal.CheckpointSpeeds[2]);
        }

        [Fact]
        public async Task UpdateRaceAnimalAsync_ShouldUpdateRaceAnimal()
        {
            // Arrange
            var updatedRaceAnimal = new RaceAnimal { RaceId = 1, AnimalId = 1, CheckpointSpeeds = new int[] { 14, 20, 30 }, FinalPosition = 1 };

            // Act
            await _repository.UpdateRaceAnimalAsync(updatedRaceAnimal);

            // Assert
            var raceAnimal = await _context.RaceAnimals.FindAsync(1, 1);
            Assert.Equal(30, raceAnimal.CheckpointSpeeds[2]);
        }

        [Fact]
        public async Task DeleteRaceAnimalAsync_ShouldRemoveRaceAnimal()
        {
            // Act
            await _repository.DeleteRaceAnimalAsync(1, 1);

            // Assert
            var raceAnimal = await _context.RaceAnimals.FindAsync(1, 1);
            Assert.Null(raceAnimal);
        }
    }


}
