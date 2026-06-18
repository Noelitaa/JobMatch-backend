using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;

    public ContractService(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<ContractAcceptResponse> AcceptContractAsync(int contractId, Guid studentId)
    {
        var contract = await _contractRepository.GetContractWithDetailsAsync(contractId);
        if (contract == null)
            throw new KeyNotFoundException("Contract not found");

        if (contract.IdStudent != studentId)
            throw new UnauthorizedAccessException("Student is not the signer of this contract");

        if (!string.Equals(contract.Status, "pending", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Contract is already {contract.Status ?? "null"}. Only pending contracts can be accepted.");

        contract.Status = "active";
        contract.AcceptedAt = DateTime.UtcNow;
        contract.UpdatedAt = DateTime.UtcNow;

        if (contract.Job == null)
            throw new InvalidOperationException("Contract does not have an associated job.");

        contract.Job.Status = "in-progress";
        await _contractRepository.UpdateAsync(contract);

        return new ContractAcceptResponse
        {
            Contract = ToContractResponse(contract),
            Job = JobMapper.ToResponse(contract.Job),
            Message = "Contract accepted successfully"
        };
    }

    public async Task<List<ContractListResponse>> GetContractsByCompanyAsync(Guid companyId, string? status)
    {
        var contracts = await _contractRepository.GetByCompanyIdAsync(companyId, status);

        return contracts.Select(c => new ContractListResponse
        {
            IdContract = c.IdContract,
            IdJob = c.IdJob,
            JobTitle = c.Job?.Title ?? string.Empty,
            IdStudent = c.IdStudent,
            StudentName = c.Student?.FullName ?? string.Empty,
            Status = c.Status ?? string.Empty,
            CreatedAt = c.CreatedAt,
            AcceptedAt = c.AcceptedAt
        }).ToList();
    }

    public async Task<ContractDetailResponse> GetContractByIdAsync(int contractId, Guid userId, string userRole)
    {
        var contract = await _contractRepository.GetContractWithDetailsAsync(contractId);
        if (contract == null)
            throw new KeyNotFoundException("Contract not found");

        var hasAccess =
            (userRole == "Student" && contract.IdStudent == userId) ||
            (userRole == "Company" && contract.IdCompany == userId);

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to view this contract");

        return new ContractDetailResponse
        {
            IdContract = contract.IdContract,
            IdApplication = contract.IdApplication,
            IdJob = contract.IdJob,
            JobTitle = contract.Job?.Title ?? string.Empty,
            IdStudent = contract.IdStudent,
            StudentName = contract.Student?.FullName ?? string.Empty,
            StudentEmail = contract.Student?.Email ?? string.Empty,
            IdCompany = contract.IdCompany,
            CompanyName = contract.Company?.CompanyName ?? string.Empty,
            Status = contract.Status ?? string.Empty,
            ContractData = contract.ContractData,
            CreatedAt = contract.CreatedAt,
            UpdatedAt = contract.UpdatedAt,
            AcceptedAt = contract.AcceptedAt
        };
    }

    private static ContractResponse ToContractResponse(Models.Entities.Contract contract)
    {
        return new ContractResponse
        {
            IdContract = contract.IdContract,
            IdApplication = contract.IdApplication,
            IdJob = contract.IdJob,
            IdStudent = contract.IdStudent,
            IdCompany = contract.IdCompany,
            Status = contract.Status ?? string.Empty,
            CreatedAt = contract.CreatedAt,
            UpdatedAt = contract.UpdatedAt,
            AcceptedAt = contract.AcceptedAt,
            ContractData = contract.ContractData
        };
    }
}
