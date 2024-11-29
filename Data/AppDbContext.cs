using Bobs_Racing.Models;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;
using Bobs_Racing.Model;
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
            modelBuilder.Entity<Race>()
                .HasKey(r => r.RaceId);

            modelBuilder.Entity<Race>()
                .HasMany(r => r.Animals)
                .WithOne(a => a.Race)
                .HasForeignKey(a => a.RaceId);

            modelBuilder.Entity<Animal>()
                .HasKey(a => a.AnimalId);
        }
    }

}
