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
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();
    public DbSet<Availability> Availabilities => Set<Availability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().ToTable("users");


        // Table name mappings 
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Job>().ToTable("jobs");
        modelBuilder.Entity<Application>().ToTable("applications");
        modelBuilder.Entity<Contract>().ToTable("contracts");
        modelBuilder.Entity<Skill>().ToTable("skills");
        modelBuilder.Entity<StudentSkill>().ToTable("student_skills");
        modelBuilder.Entity<Availability>().ToTable("availabilities");

        // Primary keys
        modelBuilder.Entity<Job>()
            .HasKey(j => j.IdJob);

        modelBuilder.Entity<Application>()
            .HasKey(a => a.IdApplication);
        

        modelBuilder.Entity<Contract>()
            .HasKey(c => c.IdContract);

        modelBuilder.Entity<Skill>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Availability>()
            .HasKey(a => a.Id);

        // Unique index: prevent duplicate applications
        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.IdJob, a.IdStudent })
            .IsUnique()
            .HasDatabaseName("IX_Application_Job_Student");

        // StudentSkill composite PK
        modelBuilder.Entity<StudentSkill>()
            .HasKey(ss => new { ss.StudentId, ss.SkillId });

        // Relationships: StudentSkill
        modelBuilder.Entity<StudentSkill>()
            .HasOne(ss => ss.Student)
            .WithMany(u => u.StudentSkills)
            .HasForeignKey(ss => ss.StudentId);

        modelBuilder.Entity<StudentSkill>()
            .HasOne(ss => ss.Skill)
            .WithMany(s => s.StudentSkills)
            .HasForeignKey(ss => ss.SkillId);

        // Availability → User
        modelBuilder.Entity<Availability>()
            .HasOne(a => a.Student)
            .WithMany(u => u.Availabilities)
            .HasForeignKey(a => a.StudentId);

        // Contract relationships
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