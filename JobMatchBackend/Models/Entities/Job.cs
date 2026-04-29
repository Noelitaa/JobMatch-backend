// Models/Entities/Job.cs
using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.Models.Entities;
public class Job
{
    [Key]
    public int IdJob { get; set; }
    public Guid IdCompany { get; set; }  // Guid según Develop
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = "fixed-time";
    public string? Status { get; set; }
    public decimal Payment { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public DateOnly WorkDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Deliverables { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties de Develop
    public virtual User? Company { get; set; }
    public virtual ICollection<Application>? Applications { get; set; }
}