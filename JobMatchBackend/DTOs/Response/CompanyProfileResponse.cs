// DTOs/Response/CompanyProfileResponse.cs
namespace JobMatchBackend.DTOs.Response;

public class CompanyProfileResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? Description { get; set; }
    public string? CompanyId { get; set; }      
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }      
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool EmailVerified { get; set; }
    public int ActiveJobsCount { get; set; }
}