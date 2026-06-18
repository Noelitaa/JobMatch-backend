using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> AddCompanyAsync(User user);
    User? GetByEmail(string email);

    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
}
