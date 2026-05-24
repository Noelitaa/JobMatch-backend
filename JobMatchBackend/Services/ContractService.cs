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
            throw new InvalidOperationException($"Contract is already {contract.Status}. Only pending contracts can be accepted.");

        contract.Status = "active";
        contract.AcceptedAt = DateTime.UtcNow;

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
