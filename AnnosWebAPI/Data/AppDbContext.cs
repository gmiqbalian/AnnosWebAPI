using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AnnosWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Advertisment> Advertisments { get; set; }
    }
}
