using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _dbContext;

    public StudentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid studentId)
    {
        return await _dbContext.User
            .Include(u => u.Availabilities)
            .Include(u => u.StudentSkills)
                .ThenInclude(ss => ss.Skill)
            .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == "Student" && u.IsActive);
    }

    public async Task<List<Skill>> GetSkillsByStudentIdAsync(Guid studentId)
    {
        return await _dbContext.StudentSkills
            .Where(ss => ss.StudentId == studentId)
            .Include(ss => ss.Skill)
            .Select(ss => ss.Skill!)
            .ToListAsync();
    }

    public async Task<Skill?> GetSkillByNameAsync(string skillName)
    {
        return await _dbContext.Skills
            .FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());
    }

    public async Task<Skill> CreateSkillAsync(Skill skill)
    {
        _dbContext.Skills.Add(skill);
        await _dbContext.SaveChangesAsync();
        return skill;
    }

    public async Task AddSkillAsync(Guid studentId, Guid skillId)
    {
        _dbContext.StudentSkills.Add(new StudentSkill
        {
            StudentId = studentId,
            SkillId = skillId
        });

        await _dbContext.SaveChangesAsync();
    }
}