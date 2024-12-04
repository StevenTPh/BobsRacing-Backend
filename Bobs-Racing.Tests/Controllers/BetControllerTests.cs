using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bobs_Racing.Controllers;
using Bobs_Racing.Repositories;
using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Tests.Controllers
{
    public class BetControllerTests
    {
        private readonly AppDbContext _context;
        private readonly BetController _controller;

        public BetControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestBetDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new BetController(new BetRepository(_context));

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
        public async Task GetAllBets_ShouldReturnOkResult()
        {
            // Act
            var result = await _controller.GetAllBets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bets = Assert.IsType<List<Bet>>(okResult.Value);
            Assert.Single(bets);
        }

        [Fact]
        public async Task GetBetById_ShouldReturnOkResult_WhenBetExists()
        {
            // Act
            var result = await _controller.GetBetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var bet = Assert.IsType<Bet>(okResult.Value);
            Assert.Equal(1, bet.BetId);
        }

        [Fact]
        public async Task GetBetById_ShouldReturnNotFound_WhenBetDoesNotExist()
        {
            // Act
            var result = await _controller.GetBetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateBet_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newBet = new Bet { BetId = 2, Amount = 30, UserId = 1, RaceId = 1, AnimalId = 1, Date = DateTime.Now, PotentialPayout = 60 };

            // Act
            var result = await _controller.CreateBet(newBet);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdBet = Assert.IsType<Bet>(createdResult.Value);
            Assert.Equal(2, createdBet.BetId);
        }

        [Fact]
        public async Task UpdateBet_ShouldReturnNoContent_WhenBetExists()
        {
            // Arrange
            var updatedBet = new Bet { BetId = 1, Amount = 60, UserId = 1, RaceId = 1, AnimalId = 1, Date = DateTime.Now, PotentialPayout = 120 };

            // Act
            var result = await _controller.UpdateBet(1, updatedBet);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify update
            var bet = await _context.Bets.FindAsync(1);
            Assert.Equal(60, bet.Amount);
        }

        [Fact]
        public async Task DeleteBet_ShouldReturnNoContent_WhenBetExists()
        {
            // Act
            var result = await _controller.DeleteBet(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var bet = await _context.Bets.FindAsync(1);
            Assert.Null(bet);
        }

        [Fact]
        public async Task DeleteBet_ShouldReturnNotFound_WhenBetDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteBet(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }


}
