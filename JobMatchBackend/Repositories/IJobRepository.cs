using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<Job?> GetByIdAsync(int id);
    Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId);
    Task<List<Job>> GetAllAsync();
    Task<Job?> GetByIdWithCompanyAsync(int jobId);
    Task UpdateAsync(Job job);
}