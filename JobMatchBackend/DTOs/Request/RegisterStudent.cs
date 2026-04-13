using System.ComponentModel.DataAnnotations;
namespace JobMatchBackend.DTOs.Request;

public class RegisterStudent
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    [MinLength(5)]
    public string? FullName { get; set; }
    [Required]
    public string? University { get; set; }
    [Required]
    public string? Career { get; set; }
    [Required]
    public string? StudentId { get; set; }
}