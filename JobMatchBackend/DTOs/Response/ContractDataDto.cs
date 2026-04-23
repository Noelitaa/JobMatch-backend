// DTOs/ContractDataDto.cs
namespace JobMatchBackend.DTOs;

public class ContractDataDto
{
    public int IdApplication { get; set; }
    public int IdJob { get; set; }
    public Guid IdStudent { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string? JobType { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyEmail { get; set; }
    public string? CompanyOwnerName { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string? StudentUniversity { get; set; }
    public string? StudentCareer { get; set; }
}