using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<List<Job>> GetAllAsync();
    Task<Job?> GetByIdWithCompanyAsync(int jobId);
}
