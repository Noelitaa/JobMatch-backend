namespace JobMatchBackend.DTOs.Response;

public class JobDetailResponse
{
    public int IdJob { get; set; }
    public Guid IdCompany { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Status { get; set; }
    public decimal Payment { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public DateOnly WorkDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<string>? Deliverables { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CompanySummaryResponse Company { get; set; } = new();
}