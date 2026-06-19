// Models/Entities/Job.cs
namespace JobMatchBackend.Models.Entities;

public class Job
{
    public int IdJob { get; set; }
    public Guid IdCompany { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Type { get; set; }  
    public string? Status { get; set; }
    
    public decimal Payment { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public DateOnly? WorkDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Deliverables { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual User? Company { get; set; }
    public virtual ICollection<Application>? Applications { get; set; }
    public virtual ICollection<Contract>? Contracts { get; set; }
}