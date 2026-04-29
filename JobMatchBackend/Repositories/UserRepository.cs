using System.Runtime.Versioning;
using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;

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
        return _dbContext.User
            .FirstOrDefault(u => u.Email == email);
    }

}