using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;

    public SkillService(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Skill?> AddSkillToStudentAsync(Guid studentId, string skillName)
    {
        var student = await _skillRepository.GetStudentWithSkillsAsync(studentId);
        if (student == null) return null;

        var skill = await _skillRepository.GetSkillByNameAsync(skillName);
        if (skill == null)
        {
            skill = new Skill 
            { 
                Id = Guid.NewGuid(), 
                Name = skillName.Trim() 
            };
            await _skillRepository.AddSkillAsync(skill);
            await _skillRepository.SaveChangesAsync();
        }

        if (!student.Skills.Any(s => s.Name.Equals(skillName.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            student.Skills.Add(skill);
            await _skillRepository.SaveChangesAsync();
        }

        return skill;
    }
}