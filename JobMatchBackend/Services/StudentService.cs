using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response.Student;
using JobMatchBackend.Mappers;
using JobMatchBackend.Models.Entities;
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

    public async Task RemoveSkillFromStudentAsync(Guid studentId, Guid skillId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var hasSkill = student.StudentSkills.Any(ss => ss.SkillId == skillId);
        if (!hasSkill)
            throw new KeyNotFoundException("Skill not found for this student");

        await _studentRepository.RemoveSkillAsync(studentId, skillId);
    }

    public async Task UpdateWeeklyAvailabilityAsync(Guid studentId, UpdateWeeklyAvailabilityRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (request.TimeBlocks.Count == 0)
            throw new InvalidOperationException("At least one time block is required");

        var newAvailabilities = request.TimeBlocks.Select(block =>
        {
            if (block.Day < 0 || block.Day > 6)
                throw new InvalidOperationException("Day must be between 0 and 6");

            if (block.StartTime >= block.EndTime)
                throw new InvalidOperationException("Start time must be before end time");

            return new Availability
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                DayOfWeek = block.Day,
                StartTime = block.StartTime,
                EndTime = block.EndTime
            };
        }).ToList();

        ValidateNoOverlaps(newAvailabilities);

        var currentAvailabilities = await _studentRepository.GetAvailabilitiesByStudentIdAsync(studentId);
        ValidateNoOverlaps(currentAvailabilities.Concat(newAvailabilities).ToList());

        await _studentRepository.AddAvailabilitiesAsync(newAvailabilities);
    }

    private static void ValidateNoOverlaps(List<Availability> availabilities)
    {
        foreach (var dayGroup in availabilities.GroupBy(availability => availability.DayOfWeek))
        {
            var orderedBlocks = dayGroup
                .OrderBy(availability => availability.StartTime)
                .ToList();

            for (var index = 1; index < orderedBlocks.Count; index++)
            {
                var previousBlock = orderedBlocks[index - 1];
                var currentBlock = orderedBlocks[index];

                if (currentBlock.StartTime < previousBlock.EndTime)
                    throw new InvalidOperationException("Time blocks cannot overlap on the same day");
            }
        }
    }
}
