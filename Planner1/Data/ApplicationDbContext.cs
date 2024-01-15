using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Planner1.Models;

namespace Planner1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Register> Register { get; set; }  
        public DbSet<PlanPurchased> PlanPurchased { get; set; }
        public DbSet<Plans> Plans { get; set; }

        private readonly IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("SQLServerConnection"));
        }
    }
}
