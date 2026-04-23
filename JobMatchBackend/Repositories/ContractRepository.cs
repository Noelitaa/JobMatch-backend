// Repositories/ContractRepository.cs
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
}