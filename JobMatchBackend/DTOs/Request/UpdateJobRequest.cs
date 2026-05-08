namespace JobMatchBackend.DTOs.Request;

public class UpdateJobRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Payment { get; set; }
    public string? PaymentType { get; set; }
    public DateOnly? WorkDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<string>? Deliverables { get; set; }
}