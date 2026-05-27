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
            .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == "Student");
    }

    public async Task<List<Skill>> GetSkillsByStudentIdAsync(Guid studentId)
    {
        var exists = await _dbContext.User
            .AnyAsync(u => u.Id == studentId && u.Role == "Student");

        if (!exists)
            throw new KeyNotFoundException("Student not found");

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

    public async Task<bool> AddSkillAsync(Guid studentId, Guid skillId)
    {
        var alreadyExists = await _dbContext.StudentSkills
            .AnyAsync(ss => ss.StudentId == studentId && ss.SkillId == skillId);

        if (alreadyExists)
            return false;

        _dbContext.StudentSkills.Add(new StudentSkill
        {
            StudentId = studentId,
            SkillId = skillId
        });

        await _dbContext.SaveChangesAsync();
        return true;
    }
}