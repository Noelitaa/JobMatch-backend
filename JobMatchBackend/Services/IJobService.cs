using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.DTOs.Request;
namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobResponse> CreateJobAsync(CreateJobRequest request);
    Task<List<JobResponse>> GetAllJobsAsync();
    Task<bool> DeleteJobAsync(int id);
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
}
