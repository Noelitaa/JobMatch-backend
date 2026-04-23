// Repositories/IJobRepository.cs
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(int id);
    Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId);
}