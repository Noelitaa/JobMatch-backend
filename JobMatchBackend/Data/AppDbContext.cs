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

        // User <-> Skill (many-to-many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Skills)
            .WithMany(s => s.Students)
            .UsingEntity<Dictionary<string, object>>(
                "student_skills",
                j => j.HasOne<Skill>().WithMany().HasForeignKey("id_skill"),
                j => j.HasOne<User>().WithMany().HasForeignKey("id_student")
            );

        // Table name mappings
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Job>().ToTable("jobs");
        modelBuilder.Entity<Application>().ToTable("applications");
        modelBuilder.Entity<Skill>().ToTable("skills");
        modelBuilder.Entity<StudentSkill>().ToTable("student_skills");
        modelBuilder.Entity<Availability>().ToTable("availabilities");

        // Primary keys
        modelBuilder.Entity<Job>()
            .HasKey(j => j.IdJob);

        modelBuilder.Entity<Application>()
            .HasKey(a => a.IdApplication);

        // Unique index: prevent duplicate applications
        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.IdJob, a.IdStudent })
            .IsUnique()
            .HasDatabaseName("IX_Application_Job_Student");

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