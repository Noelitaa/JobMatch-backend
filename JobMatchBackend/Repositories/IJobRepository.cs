using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IJobRepository
{
    Task<Job?> GetByIdWithCompanyAsync(int jobId);
}
