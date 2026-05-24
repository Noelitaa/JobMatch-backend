namespace JobMatchBackend.Services;

public interface ISkillService
{
    Task<List<string>> GetAllSkillNamesAsync();
}
