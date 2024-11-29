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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Many-to-Many Relationship
            modelBuilder.Entity<Race>()
                .HasMany(r => r.Animals)
                .WithMany(a => a.Races)
                .UsingEntity(j => j.ToTable("RaceAnimals"));

            // Configure JSON serialization for Rankings with a value comparer
            var rankingsComparer = new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2), // Compare sequences for equality
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Compute hash code
                c => c.ToList()); // Deep copy the collection

            modelBuilder.Entity<Race>()
                .Property(r => r.Rankings)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<int>>(v, new JsonSerializerOptions()))
                .Metadata.SetValueComparer(rankingsComparer); // Apply value comparer
        }
    }
}
