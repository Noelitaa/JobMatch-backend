using System.ComponentModel.DataAnnotations;
namespace JobMatchBackend.DTOs.Request;

public class RegisterStudent
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
    ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un número.")]
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