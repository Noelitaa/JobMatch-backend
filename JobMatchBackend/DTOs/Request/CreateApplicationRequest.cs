using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class CreateApplicationRequest
{
    [Required]
    public Guid IdStudent { get; set; }

    [Required]
    public int IdJob { get; set; }
}
