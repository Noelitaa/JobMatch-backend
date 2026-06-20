using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class AddSkillRequest
{
    [Required]
    [MinLength(1)]
    public string SkillName { get; set; } = string.Empty;
}