// Services/CompanyService.cs
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
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

        return CompanyMapper.ToProfileResponse(company, activeJobsCount);
    }
}