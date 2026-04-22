using System.ComponentModel.DataAnnotations;
namespace JobMatchBackend.DTOs.Request;

public class RegisterCompany
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    [MinLength(5)]
    public string? CompanyName { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? CompanyId {get; set; }
    [Required]
    public string? Phone {get; set;}
}