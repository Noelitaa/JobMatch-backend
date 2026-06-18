namespace JobMatchBackend.DTOs.Response;

public class JobResponse
{
    public int IdJob { get; set; }
    public Guid IdCompany { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public string PaymentType { get; set; } = string.Empty;

    // Fixed-time job fields (null for autonomous jobs)
    public DateOnly? WorkDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    // Autonomous job fields (null for fixed-time jobs)
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<string>? Deliverables { get; set; }

    public DateTime CreatedAt { get; set; }
}