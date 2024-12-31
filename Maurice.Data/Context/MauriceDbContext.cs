using Maurice.Core.Models;
using Maurice.Data.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Maurice.Data.Context
{
    internal class MauriceDbContext : DbContext
    {
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<RegimenFiscal> RegimenFiscal { get; set; }
        public DbSet<LimitesIsrSat> LimitesIsrSat { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "maurice.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure tables if necessary (e.g., add constraints, default values, etc.)
        }
    }
}
