// Models/Entities/Job.cs - ACTUALIZADO
namespace JobMatchBackend.Models.Entities;

public class Job
{
    public int IdJob { get; set; }
    public Guid IdCompany { get; set; }  
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public virtual User? Company { get; set; }
    public virtual ICollection<Application>? Applications { get; set; }
}