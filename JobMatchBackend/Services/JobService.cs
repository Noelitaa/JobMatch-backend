using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Repositories;
using JobMatchBackend.DTOs.Request;
namespace JobMatchBackend.Services;
using JobMatchBackend.Mappers;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobDetailResponse> GetJobByIdAsync(int jobId)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
        {
            throw new KeyNotFoundException("Job not found");
        }

        return new JobDetailResponse
        {
            IdJob = job.IdJob,
            IdCompany = job.IdCompany,
            Title = job.Title,
            Description = job.Description,
            Type = job.Type,
            Status = job.Status,
            Payment = job.Payment,
            PaymentType = job.PaymentType,
            WorkDate = job.WorkDate,
            StartTime = job.StartTime,
            EndTime = job.EndTime,
            StartDate = job.StartDate,
            EndDate = job.EndDate,
            Deliverables = job.Deliverables,
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt,
            Company = new CompanySummaryResponse
            {
                Id = job.Company?.Id ?? Guid.Empty,
                CompanyName = job.Company?.CompanyName,
                Email = job.Company?.Email ?? string.Empty,
                Phone = job.Company?.Phone,
                Description = job.Company?.Description,
                AvatarUrl = job.Company?.AvatarUrl
            }
        };
    }

    public async Task<JobResponse> CreateJobAsync(CreateJobRequest request)
{
    // Combinar fecha y hora
    var dateTimeString = $"{request.Date}T{request.StartTime}:00Z";

    // Parsear como UTC
    var jobDateTime = DateTime.Parse(
        dateTimeString,
        null,
        System.Globalization.DateTimeStyles.AdjustToUniversal
    );

    // Validar que sea futura
    if (jobDateTime <= DateTime.UtcNow)
        throw new ArgumentException("La fecha y hora del trabajo deben ser en el futuro.");

    var job = JobMapper.ToEntity(request);
    var created = await _jobRepository.CreateAsync(job);

    return JobMapper.ToResponse(created);
}
}
