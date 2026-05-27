namespace JobMatchBackend.DTOs.Response;

public class ApplicationDetailResponse
{
    public int IdApplication { get; set; }
    public string Status { get; set; } = string.Empty;
    public int IdJob { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string? JobType { get; set; }
    public Guid IdCompany { get; set; }
    public string? CompanyName { get; set; }
    public Guid IdStudent { get; set; }
    public string? StudentName { get; set; }
    public string? StudentEmail { get; set; }
    public string? StudentUniversity { get; set; }
    public string? StudentCareer { get; set; }
    public DateTime CreatedAt { get; set; }
}
