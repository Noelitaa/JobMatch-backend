using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IStudentRepository
{
    Task<User?> GetByIdAsync(Guid studentId);
    Task<List<Skill>> GetSkillsByStudentIdAsync(Guid studentId);
    Task<Skill?> GetSkillByNameAsync(string skillName);
    Task<Skill?> GetSkillByIdAsync(Guid skillId);
    Task<Skill> CreateSkillAsync(Skill skill);
    Task AddSkillAsync(Guid studentId, Guid skillId);
    Task RemoveSkillAsync(Guid studentId, Guid skillId);
}