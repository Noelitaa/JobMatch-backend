namespace JobMatchBackend.DTOs.Response;

public class ContractDetailResponse
{
    public int IdContract { get; set; }
    public int IdApplication { get; set; }
    public int IdJob { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public Guid IdStudent { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public Guid IdCompany { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ContractData { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
}
