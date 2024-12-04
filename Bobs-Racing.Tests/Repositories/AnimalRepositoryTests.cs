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
    public class AnimalRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly AnimalRepository _repository;

        public AnimalRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAnimalRepositoryDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new AnimalRepository(_context);

            // Reset the database for each test run
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
            _context.Animals.Add(new Animal { AnimalId = 1, Name = "Animal1", Image = "animal.jpg", MinSpeed = 10, MaxSpeed = 20 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAnimals()
        {
            // Act
            var animals = await _repository.GetAllAsync();

            // Assert
            Assert.Single(animals);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAnimal_WhenAnimalExists()
        {
            // Act
            var animal = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(animal);
            Assert.Equal(1, animal.AnimalId);
            Assert.Equal("Animal1", animal.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenAnimalDoesNotExist()
        {
            // Act
            var animal = await _repository.GetByIdAsync(99);

            // Assert
            Assert.Null(animal);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAnimal()
        {
            // Arrange
            var newAnimal = new Animal { AnimalId = 2, Name = "Animal2", Image = "animal.jpg", MinSpeed = 15, MaxSpeed = 25 };

            // Act
            await _repository.AddAsync(newAnimal);

            // Assert
            var animal = await _context.Animals.FindAsync(2);
            Assert.NotNull(animal);
            Assert.Equal("Animal2", animal.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAnimal()
        {
            // Arrange
            var updatedAnimal = new Animal { AnimalId = 1, Name = "UpdatedAnimal1", Image = "animal.jpg", MinSpeed = 12, MaxSpeed = 22 };

            // Act
            await _repository.UpdateAsync(updatedAnimal);

            // Assert
            var animal = await _context.Animals.FindAsync(1);
            Assert.Equal("UpdatedAnimal1", animal.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAnimal_WhenAnimalExists()
        {
            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var animal = await _context.Animals.FindAsync(1);
            Assert.Null(animal);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDoNothing_WhenAnimalDoesNotExist()
        {
            // Act
            await _repository.DeleteAsync(99);

            // Assert
            var animal = await _context.Animals.FindAsync(99);
            Assert.Null(animal);
        }
    }


}
