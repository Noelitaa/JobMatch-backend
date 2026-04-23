// Services/IApplicationService.cs
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IApplicationService
{
    Task<IEnumerable<ApplicationResponse>> GetApplicationsByJobAsync(int jobId, int companyId);
    Task<UpdateApplicationResponse> UpdateApplicationStatusAsync(int applicationId, int companyId, UpdateApplicationRequest request);
}