using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;

    public ContractService(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<List<ContractSummaryResponse>> GetContractsByUserAsync(Guid userId, string? status)
    {
        if (!string.IsNullOrEmpty(status) && status != "active" && status != "completed")
            throw new ArgumentException("Status must be 'active' or 'completed'");

        var contracts = await _contractRepository.GetByUserIdAsync(userId, status);

        return contracts.Select(ToSummaryResponse).ToList();
    }

    public async Task<ContractDetailResponse> GetContractByIdAsync(int contractId, Guid callerId)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId);
        if (contract == null)
            throw new KeyNotFoundException("Contract not found");

        if (contract.IdStudent != callerId && contract.IdCompany != callerId)
            throw new UnauthorizedAccessException("You do not have access to this contract");

        return ToDetailResponse(contract);
    }

    private static ContractSummaryResponse ToSummaryResponse(Models.Entities.Contract contract) => new()
    {
        IdContract = contract.IdContract,
        JobTitle = contract.Job?.Title,
        CompanyName = contract.Job?.Company?.CompanyName,
        StudentName = contract.Student?.FullName,
        Status = contract.Status ?? string.Empty,
        StartDate = contract.Job?.StartDate,
        EndDate = contract.Job?.EndDate,
        Payment = contract.Job?.Payment ?? 0,
        CreatedAt = contract.CreatedAt,
        AcceptedAt = contract.AcceptedAt
    };

    private static ContractDetailResponse ToDetailResponse(Models.Entities.Contract contract) => new()
    {
        IdContract = contract.IdContract,
        IdJob = contract.IdJob,
        JobTitle = contract.Job?.Title,
        JobType = contract.Job?.Type,
        StartDate = contract.Job?.StartDate,
        EndDate = contract.Job?.EndDate,
        Payment = contract.Job?.Payment ?? 0,
        PaymentType = contract.Job?.PaymentType,
        IdStudent = contract.IdStudent,
        StudentName = contract.Student?.FullName,
        IdCompany = contract.IdCompany,
        CompanyName = contract.Job?.Company?.CompanyName,
        Status = contract.Status ?? string.Empty,
        ContractData = contract.ContractData,
        CreatedAt = contract.CreatedAt,
        AcceptedAt = contract.AcceptedAt,
        UpdatedAt = contract.UpdatedAt
    };
}
