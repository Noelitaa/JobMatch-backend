using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<List<Job>> GetAllAsync();
    Task<Job?> GetByIdWithCompanyAsync(int jobId);
    Task<Job?> GetByIdAsync(int jobId);
    Task<bool> HasAcceptedApplicationsAsync(int jobId);
    Task<Job> UpdateAsync(Job job);
}