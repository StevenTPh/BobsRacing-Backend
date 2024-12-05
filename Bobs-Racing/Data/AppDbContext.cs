using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet properties
        public DbSet<Race>? Races { get; set; }
        public DbSet<Animal>? Animals { get; set; }
        public DbSet<RaceAnimal>? RaceAnimals { get; set; }
        public DbSet<Bet>? Bets { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use surrogate key for RaceAnimal
            modelBuilder.Entity<RaceAnimal>()
                .HasKey(ra => ra.RaceAnimalId);

            // Configure relationships with Race and Animal
            modelBuilder.Entity<RaceAnimal>()
                .HasOne(ra => ra.Race)
                .WithMany(r => r.RaceAnimals)
                .HasForeignKey(ra => ra.RaceId); // Foreign Key for Race

            modelBuilder.Entity<RaceAnimal>()
                .HasOne(ra => ra.Animal)
                .WithMany(a => a.RaceAnimals)
                .HasForeignKey(ra => ra.AnimalId); // Foreign Key for Animal

            // Value Converter for CheckpointSpeeds (RaceAnimal)
            modelBuilder.Entity<RaceAnimal>()
                .Property(ra => ra.CheckpointSpeeds)
                .HasConversion(
                    v => string.Join(",", v), // Convert int[] to string for storage
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() // Convert string back to int[]
                );

            // Value Converter for Rankings (Race)
            modelBuilder.Entity<Race>()
                .Property(r => r.Rankings)
                .HasConversion(
                    v => string.Join(",", v), // Convert int[] to string for storage
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() // Convert string back to int[]
                );

            // Bet relationships
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId); // Foreign Key for User

            modelBuilder.Entity<Bet>()
                .HasOne(b => b.RaceAnimal)
                .WithMany(ra => ra.Bets)
                .HasForeignKey(b => b.RaceAnimalId); // Foreign Key for RaceAnimal
        }

    }
}
