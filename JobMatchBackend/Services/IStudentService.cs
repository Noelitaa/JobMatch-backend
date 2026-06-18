using JobMatchBackend.DTOs.Response.Student;
using JobMatchBackend.DTOs.Request;

namespace JobMatchBackend.Services;

public interface IStudentService
{
    Task<StudentProfileResponse> GetStudentByIdAsync(Guid studentId);
    Task<AvailabilityResponse> GetStudentAvailabilityAsync(Guid studentId);
    Task<List<string>> GetStudentSkillsAsync(Guid studentId);
    Task<string> AddSkillToStudentAsync(Guid studentId, string skillName);
    Task RemoveSkillFromStudentAsync(Guid studentId, Guid skillId);
    Task UpdateWeeklyAvailabilityAsync(Guid studentId, UpdateWeeklyAvailabilityRequest request);
}
