using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class CreateApplicationRequest
{
    [Required]
    public int? IdJob { get; set; }
}
