using Microsoft.EntityFrameworkCore;
using RouletteApiCleanCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApiCleanCode.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Roulette> Roulette { get; set; }
    }
}
