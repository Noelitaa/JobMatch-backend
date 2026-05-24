using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Models;
namespace JobMatchBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User => Set<User>();
    public DbSet<Job> Jobs => Set<Job>();           
    public DbSet<Application> Applications => Set<Application>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Job>().ToTable("jobs");
        modelBuilder.Entity<Application>().ToTable("applications");
        
        modelBuilder.Entity<Job>()
            .HasKey(j => j.IdJob);
            
        modelBuilder.Entity<Application>()
            .HasKey(a => a.IdApplication);
        
        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.IdJob, a.IdStudent })
            .IsUnique()
            .HasDatabaseName("IX_Application_Job_Student");
    }
}
