using JobMatchBackend.DTOs.Response.Student;

namespace JobMatchBackend.Services;

public interface IStudentService
{
    Task<StudentProfileResponse> GetStudentByIdAsync(Guid studentId);
    Task<List<string>> GetStudentSkillsAsync(Guid studentId);
    Task<string> AddSkillToStudentAsync(Guid studentId, string skillName);
    Task RemoveSkillFromStudentAsync(Guid studentId, Guid skillId);
}
