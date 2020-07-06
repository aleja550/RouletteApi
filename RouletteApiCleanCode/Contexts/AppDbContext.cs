using Microsoft.EntityFrameworkCore;

using RouletteApiCleanCode.Models;

namespace RouletteApiCleanCode.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Roulette> Roulette { get; set; }
        public DbSet<RouletteUser> RouletteUser { get; set; }
        public DbSet<RouletteMatch> RouletteMatch { get; set; }
        public DbSet<RouletteBet> RouletteBet { get; set; }
    }
}
    