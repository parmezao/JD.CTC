using JD.CTC.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JD.CTC.Data.Repositories.DataContext
{
    public class CTCContext : DbContext
    {
        private readonly string _connectionString;

        public CTCContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Legado> Legado { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)        
        {
            modelBuilder.Entity<Legado>().ToTable("TBJDCTC_LEGADO");
            modelBuilder.Entity<Legado>().HasNoKey();
            modelBuilder.Entity<Legado>().Ignore(x => x.Id);
            modelBuilder.Entity<Legado>().Property(x => x.NomeLegado).HasColumnName("NMLEGADO");
            modelBuilder.Entity<Legado>().Property(x => x.SitLegado).HasColumnName("STLEGADO");
        }
    }
}
