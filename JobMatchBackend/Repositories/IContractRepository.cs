// Repositories/IContractRepository.cs
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IContractRepository
{
    Task<Contract> CreateAsync(Contract contract);
    Task<Contract?> GetContractWithDetailsAsync(int contractId);
    Task UpdateAsync(Contract contract);
}