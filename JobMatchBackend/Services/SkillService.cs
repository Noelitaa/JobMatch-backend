using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;

    public SkillService(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<List<string>> GetAllSkillsAsync()
    {
        var skills = await _skillRepository.GetAllAsync();
        return skills.Select(s => s.Name).ToList();
    }
}