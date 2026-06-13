namespace JobMatchBackend.DTOs.Request;

public class CreateJobRequest
{
    public string CompanyId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = "autonomous";
    public string Date { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<string>? Deliverables { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public List<string>? SkillsRequired { get; set; }
}