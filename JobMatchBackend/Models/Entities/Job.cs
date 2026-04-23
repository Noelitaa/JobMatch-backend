// Models/Entities/Job.cs
namespace JobMatchBackend.Models.Entities;

public class Job
{
   public int IdJob { get; set; }
    public Guid IdCompany { get; set; }  // 👈 DEBE SER Guid, NO int
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual User? Company { get; set; }
    public virtual ICollection<Application>? Applications { get; set; }
    public virtual ICollection<Contract>? Contracts { get; set; }
}