// Services/ICompanyService.cs
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface ICompanyService
{
    Task<CompanyProfileResponse> GetCompanyProfileAsync(Guid companyId);
}