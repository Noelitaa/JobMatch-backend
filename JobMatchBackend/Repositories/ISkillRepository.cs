using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface ISkillRepository
{
    Task<User?> GetStudentWithSkillsAsync(Guid studentId);
    Task<Skill?> GetSkillByNameAsync(string name);
    Task AddSkillAsync(Skill skill);
    Task AddStudentSkillRelationAsync(Guid studentId, Guid skillId);
    Task SaveChangesAsync();
}