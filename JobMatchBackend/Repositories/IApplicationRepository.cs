// Repositories/IApplicationRepository.cs
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IApplicationRepository
{
    Task<Application?> GetByIdAsync(int id);
    Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
    Task UpdateAsync(Application application);
    Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId);
}