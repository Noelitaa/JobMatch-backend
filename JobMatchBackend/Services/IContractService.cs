using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IContractService
{
    Task<ContractAcceptResponse> AcceptContractAsync(int contractId, Guid studentId);
    Task<ContractDetailResponse> GetContractByIdAsync(int contractId, Guid userId, string userRole);
}