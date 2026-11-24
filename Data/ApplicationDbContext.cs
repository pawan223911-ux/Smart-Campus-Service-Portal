using Microsoft.EntityFrameworkCore;
using SmartCampusServicePortal.Models;

namespace SmartCampusServicePortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Student>()
                .HasIndex(e => e.StudentIdNumber)
                .IsUnique();

            modelBuilder.Entity<ServiceRequest>()
                .HasKey(e => e.Id);
        }
    }
}
