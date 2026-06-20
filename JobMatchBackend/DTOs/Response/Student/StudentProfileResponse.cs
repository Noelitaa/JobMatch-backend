using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.DTOs.Response.Student;

public class StudentProfileResponse
{
    public Guid Id { get; set; }
    public UserProfileResponse User { get; set; } = new();
    public string? University { get; set; }
    public string? Career { get; set; }
    public string? StudentId { get; set; }
    public List<StudentSkillResponse> Skills { get; set; } = new();
    public AvailabilityResponse Availability { get; set; } = new();
    public float AverageRating { get; set; }
}
