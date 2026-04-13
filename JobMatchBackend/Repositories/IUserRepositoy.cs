using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
}