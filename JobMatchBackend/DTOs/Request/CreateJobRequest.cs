namespace JobMatchBackend.DTOs.Request;

public class CreateJobRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // No default: an omitted Type must trigger an explicit "type is required" error,
    // not silently fall through to fixed-time validation.
    public string Type { get; set; } = string.Empty;

    // Fixed-time job fields (required when Type == "fixed-time")
    public string Date { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;

    // Autonomous job fields (required when Type == "autonomous")
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<string>? Deliverables { get; set; }

    public string PaymentType { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public List<string>? SkillsRequired { get; set; }
}