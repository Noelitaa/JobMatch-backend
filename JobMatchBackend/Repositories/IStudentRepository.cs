using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IStudentRepository
{
    Task<User?> GetByIdAsync(Guid studentId);
}
