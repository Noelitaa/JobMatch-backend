using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobResponse> CreateJobAsync(CreateJobRequest request);
    Task<List<JobResponse>> GetAllJobsAsync();
    Task DeleteJobAsync(int id);
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
    // FIX 1: companyId added to enforce ownership verification at the service level
    Task<JobDetailResponse> UpdateJobAsync(int jobId, Guid companyId, UpdateJobRequest request);
}