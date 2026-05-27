using JobMatchBackend.DTOs;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<FcmToken> FcmTokens => Set<FcmToken>();
    public DbSet<ModerationLog> ModerationLogs => Set<ModerationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Job>().ToTable("jobs");
        modelBuilder.Entity<Application>().ToTable("applications");
        modelBuilder.Entity<Contract>().ToTable("contracts");
        modelBuilder.Entity<Skill>().ToTable("skills");
        modelBuilder.Entity<StudentSkill>().ToTable("student_skills");
        modelBuilder.Entity<Availability>().ToTable("availabilities");
        modelBuilder.Entity<Payment>().ToTable("payments");
        modelBuilder.Entity<Rating>().ToTable("ratings");
        modelBuilder.Entity<Notification>().ToTable("notifications");
        modelBuilder.Entity<FcmToken>().ToTable("fcm_tokens");
        modelBuilder.Entity<ModerationLog>().ToTable("moderation_logs");

        modelBuilder.Entity<Job>().HasKey(j => j.IdJob);
        modelBuilder.Entity<Job>().Property(j => j.Payment).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Application>().HasKey(a => a.IdApplication);
        modelBuilder.Entity<Contract>().HasKey(c => c.IdContract);
        modelBuilder.Entity<Skill>().HasKey(s => s.Id);
        modelBuilder.Entity<Availability>().HasKey(a => a.Id);
        modelBuilder.Entity<Payment>().HasKey(p => p.IdPayment);
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Rating>().HasKey(r => r.IdRating);
        modelBuilder.Entity<Notification>().HasKey(n => n.IdNotification);
        modelBuilder.Entity<FcmToken>().HasKey(f => f.IdToken);
        modelBuilder.Entity<ModerationLog>().HasKey(m => m.IdLog);

        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.IdJob, a.IdStudent })
            .IsUnique()
            .HasDatabaseName("IX_Application_Job_Student");

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Job)
            .WithMany(j => j.Applications)
            .HasForeignKey(a => a.IdJob)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.IdStudent)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentSkill>().HasKey(ss => new { ss.StudentId, ss.SkillId });

        modelBuilder.Entity<StudentSkill>()
            .HasOne(ss => ss.Student)
            .WithMany(u => u.StudentSkills)
            .HasForeignKey(ss => ss.StudentId);

        modelBuilder.Entity<StudentSkill>()
            .HasOne(ss => ss.Skill)
            .WithMany(s => s.StudentSkills)
            .HasForeignKey(ss => ss.SkillId);

        modelBuilder.Entity<Availability>()
            .HasOne(a => a.Student)
            .WithMany(u => u.Availabilities)
            .HasForeignKey(a => a.StudentId);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.IdCompany)
            .OnDelete(DeleteBehavior.Restrict);

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

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Contract)
            .WithMany()
            .HasForeignKey(p => p.IdContract)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Contract)
            .WithMany()
            .HasForeignKey(r => r.IdContract)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Rater)
            .WithMany()
            .HasForeignKey(r => r.IdRater)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Rated)
            .WithMany()
            .HasForeignKey(r => r.IdRated)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.IdContract, r.IdRater })
            .IsUnique();

        modelBuilder.Entity<Notification>()
            .Property(n => n.IsRead)
            .HasDefaultValue(false);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FcmToken>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ModerationLog>()
            .HasOne(m => m.AdminUser)
            .WithMany()
            .HasForeignKey(m => m.AdminUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ContractDataDto>()
            .HasNoKey()
            .ToView(null);
    }
}
