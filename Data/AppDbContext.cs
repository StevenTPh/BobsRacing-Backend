using Microsoft.EntityFrameworkCore;
using Bobs_Racing.Model;
using System.Text.Json;

namespace Bobs_Racing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Race> Races { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<RaceAnimal> RaceAnimals { get; set; }
        public DbSet<Bet> Bets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure RaceAnimal composite key
            modelBuilder.Entity<RaceAnimal>()
                .HasKey(ra => new { ra.RaceID, ra.AnimalID });

            // Configure Race to RaceAnimal one-to-many relationship
            modelBuilder.Entity<Race>()
                .HasMany(r => r.RaceAnimals)
                .WithOne(ra => ra.Race)
                .HasForeignKey(ra => ra.RaceID);

            // Configure Animal to RaceAnimal one-to-many relationship
            modelBuilder.Entity<Animal>()
                .HasMany(a => a.RaceAnimals)
                .WithOne(ra => ra.Animal)
                .HasForeignKey(ra => ra.AnimalID);

            // Configure Rankings as a JSON column
            modelBuilder.Entity<Race>()
                .Property(r => r.Rankings)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<int>>(v, new JsonSerializerOptions())
                );

            // Configure CheckpointSpeed as a JSON column
            modelBuilder.Entity<RaceAnimal>()
                .Property(ra => ra.CheckpointSpeed)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<int>>(v, new JsonSerializerOptions())
                );

            // Prepare Bet's relationship to RaceAnimal
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.RaceAnimal)
                .WithMany()
                .HasForeignKey(b => new { b.RaceID, b.AnimalID });
        }
    }
}
