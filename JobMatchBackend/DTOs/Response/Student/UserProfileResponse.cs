namespace JobMatchBackend.DTOs.Response.Student;

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public bool Active { get; set; }
}
