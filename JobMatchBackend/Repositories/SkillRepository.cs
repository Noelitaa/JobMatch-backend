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

    public async Task<List<Skill>> GetAllAsync()
    {
        return await _dbContext.Skills.ToListAsync();
    }
}