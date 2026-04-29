// DTOs/Response/CompanyProfileResponse.cs
namespace JobMatchBackend.DTOs.Response;

public class CompanyProfileResponse
{
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public CompanyOwnerInfo? Owner { get; set; }
    public int ActiveJobsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CompanyOwnerInfo
{
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string? OwnerEmail { get; set; }
}