using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Contract>> GetByUserIdAsync(Guid userId, string? status)
    {
        var query = _dbContext.Contracts
            .Include(c => c.Job)
                .ThenInclude(j => j!.Company)
            .Include(c => c.Student)
            .Where(c => c.IdStudent == userId || c.IdCompany == userId);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(c => c.Status == status);

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Contract?> GetByIdAsync(int contractId)
    {
        return await _dbContext.Contracts
            .Include(c => c.Job)
                .ThenInclude(j => j!.Company)
            .Include(c => c.Student)
            .FirstOrDefaultAsync(c => c.IdContract == contractId);
    }
}