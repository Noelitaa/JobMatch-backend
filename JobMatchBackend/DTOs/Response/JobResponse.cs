namespace JobMatchBackend.DTOs.Response;

public class JobResponse
{
    public int IdJob { get; set; }
    public Guid IdCompany { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Deliverables { get; set; }
    public DateTime CreatedAt { get; set; }
}