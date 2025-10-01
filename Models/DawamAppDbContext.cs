using Microsoft.EntityFrameworkCore;
using System;

namespace DawamApp.Models
{
    public class DawamAppDbContext : DbContext
    {
        public DawamAppDbContext(DbContextOptions<DawamAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed 5 sample records
            modelBuilder.Entity<EmployeeStatus>().HasData(
                new EmployeeStatus { Id = 1, EmployeeName = "Ahmed", Status = StatusType.Training, StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2025, 10, 3) },
                new EmployeeStatus { Id = 2, EmployeeName = "Sara", Status = StatusType.Vacation, StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2025, 10, 10) },
                new EmployeeStatus { Id = 3, EmployeeName = "Ali", Status = StatusType.Discharge, StartDate = new DateTime(2025, 10, 12), EndDate = new DateTime(2025, 10, 12) },
                new EmployeeStatus { Id = 4, EmployeeName = "Layla", Status = StatusType.Errand, StartDate = new DateTime(2025, 10, 15), EndDate = new DateTime(2025, 10, 15) },
                new EmployeeStatus { Id = 5, EmployeeName = "Omar", Status = StatusType.Workshop, StartDate = new DateTime(2025, 10, 20), EndDate = new DateTime(2025, 10, 22) }
            );
        }
    }
}
