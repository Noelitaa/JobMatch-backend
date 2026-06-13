using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
using JobMatchBackend.Models.Entities;
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
        if (DateTime.TryParse(request.Date + " " + request.StartTime, out var jobDateTime))
        {
            if (jobDateTime <= DateTime.Now)
                throw new ArgumentException("La fecha y hora del trabajo deben ser en el futuro.");
        }

        var job = JobMapper.ToEntity(request);
        var created = await _jobRepository.CreateAsync(job);
        return JobMapper.ToResponse(created);
    }

    public async Task<JobDetailResponse> GetJobByIdAsync(int jobId)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Job not found");

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

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(JobMapper.ToResponse).ToList();
    }

    public async Task DeleteJobAsync(int id, Guid companyId)
    {
        // FIX 2: GetByIdAsync already loads the entity with Applications — reuse it directly
        var job = await _jobRepository.GetByIdAsync(id);
        if (job == null)
            throw new KeyNotFoundException($"Job with id {id} not found");

        // FIX 1: Verify the authenticated user owns this job before deleting
        if (job.IdCompany != companyId)
            throw new UnauthorizedAccessException("Only the owning company can delete this job");

        if (job.Applications != null && job.Applications.Any())
            throw new InvalidOperationException("Cannot delete a job that has existing applications.");

        // FIX 2: Pass the entity directly — avoids the second FindAsync inside DeleteAsync
        await _jobRepository.DeleteAsync(job);
    }

    public async Task<JobDetailResponse> UpdateJobAsync(int jobId, Guid companyId, UpdateJobRequest request)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Job not found");

        if (job.IdCompany != companyId)
            throw new UnauthorizedAccessException("Company does not own this job");

        var hasAcceptedApplications = await _jobRepository.HasAcceptedApplicationsAsync(jobId);
        if (hasAcceptedApplications)
            throw new InvalidOperationException("Cannot edit a job that has accepted students");

        var hasActiveContract = await _jobRepository.HasActiveContractAsync(jobId);
        if (hasActiveContract)
            throw new InvalidOperationException("Cannot edit a job with an active contract");

        if (!string.IsNullOrWhiteSpace(request.Title)) job.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.Description)) job.Description = request.Description;
        if (request.Payment != null) job.Payment = request.Payment.Value;
        if (request.PaymentType != null) job.PaymentType = request.PaymentType;
        if (request.WorkDate != null) job.WorkDate = request.WorkDate.Value;
        if (request.StartTime != null) job.StartTime = request.StartTime.Value;
        if (request.EndTime != null) job.EndTime = request.EndTime.Value;
        if (request.StartDate != null) job.StartDate = request.StartDate;
        if (request.EndDate != null) job.EndDate = request.EndDate;
        if (request.Deliverables != null) job.Deliverables = string.Join(",", request.Deliverables);

        job.UpdatedAt = DateTime.UtcNow;

        var updated = await _jobRepository.UpdateAsync(job);

        return new JobDetailResponse
        {
            IdJob = updated.IdJob,
            IdCompany = updated.IdCompany,
            Title = updated.Title,
            Description = updated.Description,
            Type = updated.Type,
            Status = updated.Status,
            Payment = updated.Payment,
            PaymentType = updated.PaymentType,
            WorkDate = updated.WorkDate,
            StartTime = updated.StartTime,
            EndTime = updated.EndTime,
            StartDate = updated.StartDate,
            EndDate = updated.EndDate,
            Deliverables = updated.Deliverables,
            CreatedAt = updated.CreatedAt,
            UpdatedAt = updated.UpdatedAt,
            Company = new CompanySummaryResponse
            {
                Id = updated.Company?.Id ?? Guid.Empty,
                CompanyName = updated.Company?.CompanyName,
                Email = updated.Company?.Email ?? string.Empty,
                Phone = updated.Company?.Phone,
                Description = updated.Company?.Description,
                AvatarUrl = updated.Company?.AvatarUrl
            }
        };
    }
}