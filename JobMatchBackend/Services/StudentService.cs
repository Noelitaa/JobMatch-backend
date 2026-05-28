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
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var skills = await _studentRepository.GetSkillsByStudentIdAsync(studentId);
        return skills.Select(s => s.Name).ToList();
    }

    public async Task<string> AddSkillToStudentAsync(Guid studentId, string skillName)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var skill = await _studentRepository.GetSkillByNameAsync(skillName);
        if (skill == null)
            skill = await _studentRepository.CreateSkillAsync(new Models.Entities.Skill { Id = Guid.NewGuid(), Name = skillName });

        var alreadyHasSkill = student.StudentSkills.Any(ss => ss.SkillId == skill.Id);
        if (alreadyHasSkill)
            throw new InvalidOperationException("Student already has this skill");

        await _studentRepository.AddSkillAsync(studentId, skill.Id);
        return skill.Name;
    }
}