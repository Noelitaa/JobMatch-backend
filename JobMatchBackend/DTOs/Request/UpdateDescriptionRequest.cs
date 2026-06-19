using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class UpdateDescriptionRequest
{
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}
