namespace JobMatchBackend.DTOs.Response;

public class ContractSummaryResponse
{
    public int IdContract { get; set; }
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? StudentName { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
}
