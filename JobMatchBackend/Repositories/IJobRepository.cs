using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<Job?> GetByIdAsync(int id);
    Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId);
    Task<List<Job>> GetAllAsync();
    // FIX 2: Signature now receives the entity instead of the ID
    Task DeleteAsync(Job job);
    Task<Job?> GetByIdWithCompanyAsync(int jobId);
    Task<Job> UpdateAsync(Job job);
    Task<bool> HasAcceptedApplicationsAsync(int jobId);
    Task<bool> HasActiveContractAsync(int jobId);
}
