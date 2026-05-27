// DTOs/Response/ContractResponse.cs
namespace JobMatchBackend.DTOs.Response;

public class ContractResponse
{
    public int IdContract { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ContractData { get; set; }
}