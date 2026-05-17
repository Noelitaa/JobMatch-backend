using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.DTOs;

namespace JobMatchBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User => Set<User>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ContractDataDto> ContractData => Set<ContractDataDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Job>()
            .HasKey(j => j.IdJob);

        modelBuilder.Entity<Application>()
            .HasKey(a => a.IdApplication);

        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.IdJob, a.IdStudent })
            .IsUnique()
            .HasDatabaseName("IX_Application_Job_Student");

        modelBuilder.Entity<Contract>()
            .HasKey(c => c.IdContract);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Application)
            .WithOne(a => a.Contract)
            .HasForeignKey<Contract>(c => c.IdApplication)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Job)
            .WithMany(j => j.Contracts)
            .HasForeignKey(c => c.IdJob)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Student)
            .WithMany(u => u.StudentContracts)
            .HasForeignKey(c => c.IdStudent)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Company)
            .WithMany(u => u.CompanyContracts)
            .HasForeignKey(c => c.IdCompany)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ContractDataDto>().HasNoKey();
    }
}