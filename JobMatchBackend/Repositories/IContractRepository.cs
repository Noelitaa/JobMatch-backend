using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IContractRepository
{
    Task<Contract> CreateAsync(Contract contract);
    Task<List<Contract>> GetByUserIdAsync(Guid userId, string? status);
    Task<Contract?> GetByIdAsync(int contractId);
}