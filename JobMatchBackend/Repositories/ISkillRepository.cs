using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface ISkillRepository
{
    Task<List<Skill>> GetAllAsync();
}
