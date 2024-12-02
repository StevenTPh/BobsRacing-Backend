using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bobs_Racing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet for Race and Animal
        public DbSet<Race> Races { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<RaceAnimal> RaceAnimals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            // Value converter for CheckpointSpeeds
            modelBuilder.Entity<RaceAnimal>()
                .Property(ra => ra.CheckpointSpeeds)
                .HasConversion(
                    v => string.Join(",", v),         // Convert int[] to string for the database
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() // Convert string back to int[]
                );
        }

    }

}
