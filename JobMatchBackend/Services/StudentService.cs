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

    public async Task<List<string>> GetStudentSkillsAsync(Guid studentId)
    {
        var skills = await _studentRepository.GetSkillsByStudentIdAsync(studentId);
        return skills.Select(s => s.Name).ToList();
    }

    public async Task<string> AddSkillToStudentAsync(Guid studentId, string skillName)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var skill = await _studentRepository.GetOrCreateSkillAsync(skillName);

        var added = await _studentRepository.AddSkillAsync(studentId, skill.Id);
        if (!added)
            throw new InvalidOperationException("Student already has this skill");

        return skill.Name;
    }
}