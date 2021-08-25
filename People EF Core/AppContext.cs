using Microsoft.EntityFrameworkCore;

namespace People_EF_Core
{
    public sealed class AppContext : DbContext
    {
        public AppContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<User> User { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<Сourier> Сouriers { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = mydb; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>().HasOne(w => w.Company).WithMany(c => c.Workers)
                .HasForeignKey(w => w.CompanyId);
        }
    }
}