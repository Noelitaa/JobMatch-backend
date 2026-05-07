using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IJobService
{
    Task<JobDetailResponse> GetJobByIdAsync(int jobId);
}
