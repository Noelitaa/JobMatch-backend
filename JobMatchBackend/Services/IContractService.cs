using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IContractService
{
    Task<ContractAcceptResponse> AcceptContractAsync(int contractId, Guid studentId);
}