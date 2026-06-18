namespace JobMatchBackend.DTOs.Response;

public class ContractListResponse
{
    public int IdContract { get; set; }
    public int IdJob { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public Guid IdStudent { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
}
