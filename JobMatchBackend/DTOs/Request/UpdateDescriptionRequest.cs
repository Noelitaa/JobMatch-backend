using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class UpdateDescriptionRequest
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
