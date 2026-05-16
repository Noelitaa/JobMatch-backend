// Repositories/ICompanyRepository.cs
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface ICompanyRepository
{
    Task<User?> GetCompanyByIdAsync(Guid companyId);
    Task<int> GetActiveJobsCountAsync(Guid companyId);
    Task<List<User>> GetAllCompaniesAsync();  

}