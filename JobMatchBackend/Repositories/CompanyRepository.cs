// Repositories/CompanyRepository.cs
using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _dbContext;

    public CompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetCompanyByIdAsync(Guid companyId)
    {
        return await _dbContext.User
            .FirstOrDefaultAsync(u => u.Id == companyId && u.Role == "Company");
    }

    public async Task<int> GetActiveJobsCountAsync(Guid companyId)
    {
        return await _dbContext.Jobs
            .CountAsync(j => j.IdCompany == companyId && j.Status == "published");
    }

    public async Task<List<User>> GetAllCompaniesAsync()
    {
        return await _dbContext.User
            .Where(u => u.Role == "Company")
            .OrderBy(u => u.CompanyName)
            .ToListAsync();
    }

}