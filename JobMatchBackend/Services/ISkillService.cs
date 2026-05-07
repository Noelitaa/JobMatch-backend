using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Services;

public interface ISkillService
{
    Task<Skill?> AddSkillToStudentAsync(Guid studentId, string skillName);
}