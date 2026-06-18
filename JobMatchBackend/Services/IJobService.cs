using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobResponse> CreateJobAsync(CreateJobRequest request);
    Task<List<JobResponse>> GetAllJobsAsync();
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
    Task<JobDetailResponse> UpdateJobAsync(int jobId, Guid companyId, UpdateJobRequest request);
    Task CancelJobAsync(int jobId, Guid companyId);
}