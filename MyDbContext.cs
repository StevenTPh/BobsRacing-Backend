using Bobs_Racing.Models;
using Microsoft.EntityFrameworkCore;

namespace Bobs_Racing
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
