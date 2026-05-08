using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
    Task<JobDetailResponse> UpdateJobAsync(int jobId, UpdateJobRequest request);
}