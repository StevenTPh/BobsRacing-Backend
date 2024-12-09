using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Bobs_Racing.Data
{
    public class AppDbContext : DbContext
    {

        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options) {
            _configuration = configuration;
        }

        // DbSet properties
        public DbSet<Race>? Races { get; set; }
        public DbSet<Athlete>? Athletes { get; set; }
        public DbSet<RaceAthlete>? RaceAthletes { get; set; }
        public DbSet<Bet>? Bets { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            var adminPassword = _configuration["AdminSettings:Password"];
            if (string.IsNullOrEmpty(adminPassword))
            {
                throw new Exception("Admin password not configured in appsettings.json.");
            }

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Profilename = "Admin",
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    Role = "Admin",
                    Credits = 0
                }
            );

            // Use surrogate key for RaceAthlete
            modelBuilder.Entity<RaceAthlete>()
                .HasKey(ra => ra.RaceAthleteId);

            // Configure relationships with Race and Athlete
            modelBuilder.Entity<RaceAthlete>()
                .HasOne(ra => ra.Race)
                .WithMany(r => r.RaceAthletes)
                .HasForeignKey(ra => ra.RaceId); // Foreign Key for Race

            modelBuilder.Entity<RaceAthlete>()
                .HasOne(ra => ra.Athlete)
                .WithMany(a => a.RaceAthletes)
                .HasForeignKey(ra => ra.AthleteId); // Foreign Key for Athlete

            // Bet relationships
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId); // Foreign Key for User

            modelBuilder.Entity<Bet>()
                .HasOne(b => b.RaceAthlete)
                .WithMany(ra => ra.Bets)
                .HasForeignKey(b => b.RaceAthleteId); // Foreign Key for RaceAnimal
        }

    }
}
