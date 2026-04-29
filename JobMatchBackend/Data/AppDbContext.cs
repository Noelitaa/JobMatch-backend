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

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(a => a.IdApplication);
            entity.HasIndex(a => new { a.IdJob, a.IdStudent })
                .IsUnique()
                .HasDatabaseName("IX_Application_Job_Student");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable("jobs");
            entity.HasKey(j => j.IdJob);
            entity.Property(j => j.IdJob).HasColumnName("id_job");
            entity.Property(j => j.IdCompany).HasColumnName("id_company");
            entity.Property(j => j.Title).HasColumnName("title");
            entity.Property(j => j.Description).HasColumnName("description");
            entity.Property(j => j.Type).HasColumnName("type");
            entity.Property(j => j.Status).HasColumnName("status");
            entity.Property(j => j.Payment).HasColumnName("payment").HasPrecision(18, 2);
            entity.Property(j => j.PaymentType).HasColumnName("payment_type");
            entity.Property(j => j.WorkDate).HasColumnName("work_date");
            entity.Property(j => j.StartTime).HasColumnName("start_time");
            entity.Property(j => j.EndTime).HasColumnName("end_time");
            entity.Property(j => j.StartDate).HasColumnName("start_date");
            entity.Property(j => j.EndDate).HasColumnName("end_date");
            entity.Property(j => j.Deliverables).HasColumnName("deliverables");
            entity.Property(j => j.CreatedAt).HasColumnName("created_at");
            entity.Property(j => j.UpdatedAt).HasColumnName("updated_at");
        });
    }
}