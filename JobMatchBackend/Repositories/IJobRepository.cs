using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<Job?> GetByIdAsync(int id);
    Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId);
    Task<List<Job>> GetAllAsync();
    Task DeleteAsync(int id);
    Task<Job?> GetByIdWithCompanyAsync(int jobId);
    Task<bool> HasAcceptedApplicationsAsync(int jobId);
    Task<Job> UpdateAsync(Job job);
}