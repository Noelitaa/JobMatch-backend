// Services/CompanyService.cs
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyProfileResponse> GetCompanyProfileAsync(Guid companyId)
    {
        var company = await _companyRepository.GetCompanyByIdAsync(companyId);

        if (company == null)
            throw new KeyNotFoundException("Company not found");

        var activeJobsCount = await _companyRepository.GetActiveJobsCountAsync(companyId);

        return new CompanyProfileResponse
        {
            CompanyId = company.Id,
            CompanyName = company.CompanyName ?? string.Empty,
            Description = company.Description,
            ContactEmail = company.Email,
            ContactPhone = company.Phone,
            Owner = new CompanyOwnerInfo
            {
                OwnerId = company.Id,
                OwnerName = company.FullName ?? company.CompanyName ?? string.Empty,
                OwnerEmail = company.Email
            },
            ActiveJobsCount = activeJobsCount,
            CreatedAt = company.CreatedAt
        };
    }
}