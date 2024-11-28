using Bobs_Racing.Model;
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
                .HasMany(r => r.Animals)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                "RaceAnimal",
                r => r.HasOne<Animal>().WithMany().HasForeignKey("AnimalId"),
                    a => a.HasOne<Race>().WithMany().HasForeignKey("RaceId")
                );
        }
    }
}
