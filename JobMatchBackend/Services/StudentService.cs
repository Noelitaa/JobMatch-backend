using JobMatchBackend.DTOs.Response.Student;
using JobMatchBackend.Mappers;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentProfileResponse> GetStudentByIdAsync(Guid studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        return StudentMapper.ToResponse(student);
    }
}
