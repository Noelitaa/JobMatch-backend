using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.DTOs.Request;
namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobResponse> CreateJobAsync(CreateJobRequest request);
    Task<List<JobResponse>> GetAllJobsAsync();
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
    Task<JobDetailResponse> UpdateJobAsync(int jobId, UpdateJobRequest request);
}