using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IStudentRepository
{
    Task<User?> GetByIdAsync(Guid studentId);
    Task<List<Skill>> GetSkillsByStudentIdAsync(Guid studentId);
    Task<Skill> GetOrCreateSkillAsync(string skillName);
    Task<bool> AddSkillAsync(Guid studentId, Guid skillId);
}