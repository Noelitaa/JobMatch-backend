// DTOs/Response/ApplicationResponse.cs
namespace JobMatchBackend.DTOs.Response;

public class ApplicationResponse
{
    public int IdApplication { get; set; }
    public int IdJob { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public Guid IdStudent { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string? StudentUniversity { get; set; }
    public string? StudentCareer { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}