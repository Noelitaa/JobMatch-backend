using System.ComponentModel.DataAnnotations;
namespace JobMatchBackend.DTOs.Request;

public class RegisterCompany
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
    public string? CompanyName { get; set; }
    [Required]
    [MaxLength(500)]
    [MinLength(10)]
    public string? Description { get; set; }
    [Required]
    public string? CompanyId { get; set; }
    [Required]
    [Phone]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El telefono debe de ser de 8 digitos")]
    public string? Phone { get; set; }
}