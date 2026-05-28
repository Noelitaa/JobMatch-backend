namespace JobMatchBackend.DTOs.Response;

public class ContractDetailResponse
{
    public int IdContract { get; set; }
    public int IdJob { get; set; }
    public string? JobTitle { get; set; }
    public string? JobType { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal Payment { get; set; }
    public string? PaymentType { get; set; }
    public Guid IdStudent { get; set; }
    public string? StudentName { get; set; }
    public Guid IdCompany { get; set; }
    public string? CompanyName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ContractData { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
