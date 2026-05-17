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
}
