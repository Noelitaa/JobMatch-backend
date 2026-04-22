using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User => Set<User>();
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Many-to-Many relationship between User and Skill
        modelBuilder.Entity<User>()
            .HasMany(u => u.Skills)
            .WithMany(s => s.Students)
            .UsingEntity<Dictionary<string, object>>(
                "student_skills",
                j => j.HasOne<Skill>().WithMany().HasForeignKey("id_skill"),
                j => j.HasOne<User>().WithMany().HasForeignKey("id_student")
            );
    }
}