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
        var jobDateTime = DateTime.Parse(request.Date + " " + request.StartTime);
        if (jobDateTime <= DateTime.UtcNow)
            throw new ArgumentException("La fecha y hora del trabajo deben ser en el futuro.");

        var job = JobMapper.ToEntity(request);
        var created = await _jobRepository.CreateAsync(job);
        return JobMapper.ToResponse(created);
    }

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(JobMapper.ToResponse).ToList();
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        return await _jobRepository.DeleteAsync(id);
    }
}