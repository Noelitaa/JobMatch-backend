using JobMatchBackend.DTOs.Response;
using JobMatchBackend.DTOs.Request;
namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
    Task<JobResponse> CreateJobAsync(CreateJobRequest request);
}
