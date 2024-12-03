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
            // Composite key for RaceAnimal
            modelBuilder.Entity<RaceAnimal>()
                .HasKey(ra => new { ra.RaceId, ra.AnimalId });

            modelBuilder.Entity<RaceAnimal>()
                .HasOne(ra => ra.Race)
                .WithMany(r => r.RaceAnimals)
                .HasForeignKey(ra => ra.RaceId);

            modelBuilder.Entity<RaceAnimal>()
                .HasOne(ra => ra.Animal)
                .WithMany(a => a.RaceAnimals)
                .HasForeignKey(ra => ra.AnimalId);

            // Value Converter for CheckpointSpeeds
            modelBuilder.Entity<RaceAnimal>()
                .Property(ra => ra.CheckpointSpeeds)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
                );

            // Value Converter for Rankings in Race
            modelBuilder.Entity<Race>()
                .Property(r => r.Rankings)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                );

            // Bet relationships
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);

            // Updated composite foreign key for Bet → RaceAnimal
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.RaceAnimal)
                .WithMany(ra => ra.Bets)
                .HasForeignKey(b => new { b.RaceId, b.AnimalId });
        }
    }
}
