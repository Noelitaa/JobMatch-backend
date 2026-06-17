using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly AppDbContext _dbContext;

    public ContractRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Contract> CreateAsync(Contract contract)
    {
        _dbContext.Contracts.Add(contract);
        await _dbContext.SaveChangesAsync();
        return contract;
    }

    public async Task<Contract?> GetContractWithDetailsAsync(int contractId)
    {
        return await _dbContext.Contracts
            .Include(c => c.Job)
            .FirstOrDefaultAsync(c => c.IdContract == contractId);
    }

    public async Task UpdateAsync(Contract contract)
    {
        _dbContext.Contracts.Update(contract);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Contract>> GetByCompanyIdAsync(Guid companyId, string? status)
    {
        var query = _dbContext.Contracts
            .Include(c => c.Job)
            .Include(c => c.Student)
            .Where(c => c.IdCompany == companyId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.Status == status);

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
