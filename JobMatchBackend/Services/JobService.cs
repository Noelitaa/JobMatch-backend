using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobResponse> CreateJobAsync(CreateJobRequest request)
    {
        var JobDateTime = DateTime.Parse(request.Date + " " + request.StartTime);
        if (JobDateTime <= DateTime.UtcNow)
            throw new ArgumentException("La fecha y hora del trabajo deben ser en el futuro.");

        var Job = JobMapper.ToEntity(request);
        var Created = await _jobRepository.CreateAsync(Job);
        return JobMapper.ToResponse(Created);
    }

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var Jobs = await _jobRepository.GetAllAsync();
        return Jobs.Select(JobMapper.ToResponse).ToList();
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        return await _jobRepository.DeleteAsync(id);
    }
}