using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IContractService
{
    Task<List<ContractSummaryResponse>> GetContractsByUserAsync(Guid userId, string? status);
    Task<ContractDetailResponse> GetContractByIdAsync(int contractId, Guid callerId);
}
