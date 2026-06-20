// DTOs/Response/ContractResponse.cs
namespace JobMatchBackend.DTOs.Response;

public class ContractResponse
{
    public int IdContract { get; set; }
    public int IdApplication { get; set; }
    public int IdJob { get; set; }
    public Guid IdStudent { get; set; }
    public Guid IdCompany { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public string? ContractData { get; set; }
}
