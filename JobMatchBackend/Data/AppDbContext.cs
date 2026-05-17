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
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();
    public DbSet<Availability> Availabilities => Set<Availability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Job
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(j => j.IdJob);
            entity.Property(j => j.Payment).HasPrecision(18, 2);
        });

        // Application
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(a => a.IdApplication);
            entity.HasIndex(a => new { a.IdJob, a.IdStudent })
                .IsUnique()
                .HasDatabaseName("IX_Application_Job_Student");
        });

        // Skill
        modelBuilder.Entity<Skill>()
            .HasKey(s => s.Id);

        // StudentSkill composite PK
        modelBuilder.Entity<StudentSkill>()
            .HasKey(ss => new { ss.StudentId, ss.SkillId });

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
    }
}