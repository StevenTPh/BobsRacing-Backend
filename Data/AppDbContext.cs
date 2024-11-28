using Bobs_Racing.Model;
using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Race> Races { get; set; }
        public DbSet<Animal> Animals { get; set; }

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
