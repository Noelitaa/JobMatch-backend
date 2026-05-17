using JobMatchBackend.DTOs.Response.Student;

namespace JobMatchBackend.Services;

public interface IStudentService
{
    Task<StudentProfileResponse> GetStudentByIdAsync(Guid studentId);
}
