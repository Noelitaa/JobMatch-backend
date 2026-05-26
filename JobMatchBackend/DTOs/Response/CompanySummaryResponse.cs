namespace JobMatchBackend.DTOs.Response;

public class CompanySummaryResponse
{
    public Guid Id { get; set; }
    public string? CompanyName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
}
