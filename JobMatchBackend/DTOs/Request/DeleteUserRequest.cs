using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class DeleteUserRequest
{
    [Required]
    public string Password { get; set; } = string.Empty;
}
