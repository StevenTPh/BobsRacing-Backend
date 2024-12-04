using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
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

    public class BetRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly BetRepository _repository;

        public BetRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestBetRepositoryDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new BetRepository(_context);

            // Seed data
            var user = new User { UserId = 1, Name = "User1", Credits = 100, Password = "password" };
            var race = new Race { RaceId = 1, Date = DateTime.Now };
            var animal = new Animal { AnimalId = 1, Name = "Animal1", MinSpeed = 10, MaxSpeed = 20 };

            _context.Users.Add(user);
            _context.Races.Add(race);
            _context.Animals.Add(animal);
            _context.RaceAnimals.Add(new RaceAnimal { RaceId = 1, AnimalId = 1, CheckpointSpeeds = new int[] { 10, 15, 20 }, FinalPosition = 1 });
            _context.Bets.Add(new Bet { BetId = 1, Amount = 50, UserId = 1, RaceId = 1, AnimalId = 1, Date = DateTime.Now, PotentialPayout = 100 });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllBetsAsync_ShouldReturnAllBets()
        {
            // Act
            var bets = await _repository.GetAllBetsAsync();

            // Assert
            Assert.Single(bets);
        }

        [Fact]
        public async Task GetBetByIdAsync_ShouldReturnBet_WhenBetExists()
        {
            // Act
            var bet = await _repository.GetBetByIdAsync(1);

            // Assert
            Assert.NotNull(bet);
            Assert.Equal(1, bet.BetId);
        }

        [Fact]
        public async Task GetBetByIdAsync_ShouldReturnNull_WhenBetDoesNotExist()
        {
            // Act
            var bet = await _repository.GetBetByIdAsync(99);

            // Assert
            Assert.Null(bet);
        }

        [Fact]
        public async Task AddBetAsync_ShouldAddBet()
        {
            // Arrange
            var newBet = new Bet { BetId = 2, Amount = 30, UserId = 1, RaceId = 1, AnimalId = 1, Date = DateTime.Now, PotentialPayout = 60 };

            // Act
            await _repository.AddBetAsync(newBet);

            // Assert
            var bet = await _context.Bets.FindAsync(2);
            Assert.NotNull(bet);
            Assert.Equal(30, bet.Amount);
        }

        [Fact]
        public async Task UpdateBetAsync_ShouldUpdateBet()
        {
            // Arrange
            var updatedBet = new Bet { BetId = 1, Amount = 60, UserId = 1, RaceId = 1, AnimalId = 1, Date = DateTime.Now, PotentialPayout = 120 };

            // Act
            await _repository.UpdateBetAsync(updatedBet);

            // Assert
            var bet = await _context.Bets.FindAsync(1);
            Assert.Equal(60, bet.Amount);
        }

        [Fact]
        public async Task DeleteBetAsync_ShouldRemoveBet_WhenBetExists()
        {
            // Act
            await _repository.DeleteBetAsync(1);

            // Assert
            var bet = await _context.Bets.FindAsync(1);
            Assert.Null(bet);
        }

        [Fact]
        public async Task DeleteBetAsync_ShouldDoNothing_WhenBetDoesNotExist()
        {
            // Act
            await _repository.DeleteBetAsync(99);

            // Assert
            var bet = await _context.Bets.FindAsync(99);
            Assert.Null(bet);
        }
    }


}
