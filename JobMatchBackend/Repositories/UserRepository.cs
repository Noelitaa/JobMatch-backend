using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user)
    {
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> AddCompanyAsync(User user)
    {
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public User? GetByEmail(string email)
    {
        return _dbContext.User.FirstOrDefault(u => u.Email == email && u.IsActive);
    }


    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _dbContext.User
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.User.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
