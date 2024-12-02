using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<RaceAnimal> RaceAnimals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite foreign key for Bet
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.Race)
                .WithMany()
                .HasForeignKey(b => b.RaceID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bet>()
                .HasOne(b => b.Animal)
                .WithMany()
                .HasForeignKey(b => b.AnimalID)
                .OnDelete(DeleteBehavior.Cascade);

            // Define composite foreign key
            modelBuilder.Entity<Bet>()
                .HasIndex(b => new { b.RaceID, b.AnimalID })
                .IsUnique(); // Enforces that RaceID and AnimalID combination must be unique
        }
    }

}

