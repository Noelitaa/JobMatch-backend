// Repositories/IContractRepository.cs
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IContractRepository
{
    Task<Contract> CreateAsync(Contract contract);
}