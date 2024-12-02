using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;

namespace DATEX_ProjectDatabase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectManagers> ProjectManagers { get; set; }
        public DbSet<VOCAnalysis> VocAnalyses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ProjectDurationInDays and ProjectDurationInMonths as computed columns
            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectDurationInDays)
                .HasComputedColumnSql("DATEDIFF(day, ProjectStartDate, ProjectEndDate)");

            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectDurationInMonths)
                .HasComputedColumnSql("DATEDIFF(month, ProjectStartDate, ProjectEndDate)");

            // Configure the relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<Project>()
                .Property(p => p.NumberOfResources)
                .HasDefaultValue(null)
                .IsRequired(false);

            modelBuilder.Entity<Project>()
        .HasOne(p => p.ProjectManagerDetails) // Define one-to-one relationship
        .WithMany(pm => pm.Projects) // Each manager can have multiple projects
        .HasForeignKey(p => p.ProjectManager) // Foreign key in Project
        .HasPrincipalKey(pm => pm.Name) // Principal key in ProjectManagers
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
