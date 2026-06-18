using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IContractService
{
    Task<ContractAcceptResponse> AcceptContractAsync(int contractId, Guid studentId);
    Task<List<ContractListResponse>> GetContractsByCompanyAsync(Guid companyId, string? status);
    Task<List<ContractListResponse>> GetContractsByStudentAsync(Guid studentId, string? status);
    Task<ContractDetailResponse> GetContractByIdAsync(int contractId, Guid userId, string userRole);
}
