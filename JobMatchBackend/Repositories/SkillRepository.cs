using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _dbContext;

    public SkillRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetStudentWithSkillsAsync(Guid studentId)
    {
        return await _dbContext.User
            .Include(u => u.Skills)
            .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == "Student");
    }

    public async Task<Skill?> GetSkillByNameAsync(string name)
    {
        var normalized = name.Trim().ToLower();
        return await _dbContext.Skills
            .FirstOrDefaultAsync(s => s.Name.ToLower() == normalized);
    }

    public async Task AddSkillAsync(Skill skill)
    {
        await _dbContext.Skills.AddAsync(skill);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}