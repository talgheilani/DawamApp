using Microsoft.EntityFrameworkCore;

namespace DawamApp.Models
{
    public class DawamAppDbContext : DbContext
    {
        public DawamAppDbContext(DbContextOptions<DawamAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ✅ No seeding — database starts empty
        }
    }
}
