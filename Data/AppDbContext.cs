using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Race> Races { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<RaceAnimal> RaceAnimals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for RaceAnimal
            modelBuilder.Entity<RaceAnimal>()
                .HasKey(ra => new { ra.RaceId, ra.AnimalId });

            // Many-to-many relationships
            modelBuilder.Entity<RaceAnimal>()
                .HasOne(ra => ra.Race)
                .WithMany(r => r.RaceAnimals)
                .HasForeignKey(ra => ra.RaceId);

            modelBuilder.Entity<RaceAnimal>()
                .HasOne(ra => ra.Animal)
                .WithMany(a => a.RaceAnimals)
                .HasForeignKey(ra => ra.AnimalId);

            modelBuilder.Entity<RaceAnimal>()
                .Property(ra => ra.CheckpointSpeeds)
                .HasConversion(
                    v => string.Join(",", v),         // Convert int[] to a string
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() // Convert string back to int[]
                );
        }

    }
}
