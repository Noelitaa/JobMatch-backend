using JobMatchBackend.Models.Entities;
using JobMatchBackend.DTOs;

namespace JobMatchBackend.Repositories;

public interface IApplicationRepository
{
    Task<Application?> GetByIdAsync(int id);
    Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
    Task UpdateAsync(Application application);
    Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId);
    Task<Application?> GetApplicationWithDetailsAsync(int applicationId);  

    Task<ContractDataDto?> GetContractDataAsync(int applicationId);
}