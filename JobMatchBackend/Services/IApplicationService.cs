// Services/IApplicationService.cs
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IApplicationService
{
    Task<IEnumerable<ApplicationResponse>> GetApplicationsByJobAsync(int jobId, Guid companyId);
    Task<UpdateApplicationResponse> UpdateApplicationStatusAsync(int applicationId, Guid companyId, UpdateApplicationRequest request);
    Task<CreateApplicationResponse> CreateApplicationAsync(CreateApplicationRequest request);
}