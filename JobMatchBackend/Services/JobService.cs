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

        return ToDetailResponse(job);
    }

    public async Task<JobDetailResponse> UpdateJobAsync(int jobId, Guid companyId, UpdateJobRequest request)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Job not found");

        // Verify ownership using the entity already loaded above (no extra query needed)
        if (job.IdCompany != companyId)
            throw new UnauthorizedAccessException("You do not have permission to edit this job");

        var hasAccepted = await _jobRepository.HasAcceptedApplicationsAsync(jobId);
        if (hasAccepted)
            throw new InvalidOperationException("Cannot edit a job that has accepted applicants");

        var hasActiveContract = await _jobRepository.HasActiveContractAsync(jobId);
        if (hasActiveContract)
            throw new InvalidOperationException("Cannot edit a job with an active contract");

        if (!string.IsNullOrWhiteSpace(request.Title)) job.Title = request.Title;
        if (request.Description != null) job.Description = request.Description;
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

        return ToDetailResponse(updated);
    }

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(JobMapper.ToResponse).ToList();
    }

    // Single private method for building JobDetailResponse, avoids duplicating the mapping
    private static JobDetailResponse ToDetailResponse(Job job)
    {
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
}