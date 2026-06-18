using System.Text.Json;
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Mappers;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICompanyRepository _companyRepository;

    public JobService(IJobRepository jobRepository, INotificationRepository notificationRepository, ICompanyRepository companyRepository)
    {
        _jobRepository = jobRepository;
        _notificationRepository = notificationRepository;
        _companyRepository = companyRepository;
    }

    public async Task<JobResponse> CreateJobAsync(Guid companyId, CreateJobRequest request)
    {
        var company = await _companyRepository.GetCompanyByIdAsync(companyId);
        if (company == null)
            throw new KeyNotFoundException("Company not found");

        if (!company.IsActive)
            throw new UnauthorizedAccessException("Company is not active.");

        if (company.Role != "Company")
            throw new UnauthorizedAccessException("Authenticated user is not a company.");

        var errors = new List<string>();
        TimeOnly startTime = TimeOnly.MinValue;
        TimeOnly endTime = TimeOnly.MinValue;

        // --- Type validation (explicit allow-list, no implicit fallback) ---
        var allowedTypes = new[] { "autonomous", "fixed-time" };
        if (string.IsNullOrWhiteSpace(request.Type) ||
            !allowedTypes.Contains(request.Type, StringComparer.OrdinalIgnoreCase))
            errors.Add("The 'type' field is required and must be 'autonomous' or 'fixed-time'.");

        var isAutonomous = string.Equals(request.Type, "autonomous", StringComparison.OrdinalIgnoreCase);

        // --- Common validation (applies to every job type) ---
        if (string.IsNullOrWhiteSpace(request.Title))
            errors.Add("The 'title' field is required.");

        if (request.Payment <= 0)
            errors.Add("The 'payment' field must be greater than 0.");

        if (string.IsNullOrWhiteSpace(request.PaymentType) ||
            (request.PaymentType != "one_time" && request.PaymentType != "monthly"))
            errors.Add("The 'paymentType' field must be 'one_time' or 'monthly'.");

        if (isAutonomous)
        {
            // --- Autonomous-specific validation ---
            if (request.StartDate == null)
                errors.Add("The 'startDate' field is required.");

            if (request.EndDate == null)
                errors.Add("The 'endDate' field is required.");

            if (request.Deliverables == null || request.Deliverables.Count == 0)
                errors.Add("The 'deliverables' field is required and must contain at least one item.");
        }
        else
        {
            // --- Fixed-time-specific validation ---
            if (string.IsNullOrWhiteSpace(request.Date) || !DateOnly.TryParse(request.Date, out _))
                errors.Add("The 'date' field is required and must be a valid date.");

            if (string.IsNullOrWhiteSpace(request.StartTime) || !TimeOnly.TryParse(request.StartTime, out startTime))
                errors.Add("The 'startTime' field is required and must be a valid time.");

            if (string.IsNullOrWhiteSpace(request.EndTime) || !TimeOnly.TryParse(request.EndTime, out endTime))
                errors.Add("The 'endTime' field is required and must be a valid time.");
        }

        if (errors.Count > 0)
            throw new ArgumentException(string.Join(" ", errors));

        if (isAutonomous)
        {
            // StartDate and EndDate are guaranteed non-null past validation
            if (request.StartDate!.Value > request.EndDate!.Value)
                throw new ArgumentException("'startDate' must be on or before 'endDate'.");

            if (request.EndDate.Value < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("'endDate' must be in the future.");
        }
        else
        {
            if (DateTime.TryParse(request.Date + " " + request.StartTime, out var jobDateTime))
            {
                if (jobDateTime <= DateTime.UtcNow)
                    throw new ArgumentException("The job date and time must be in the future.");
            }

            if (startTime >= endTime)
                throw new ArgumentException("'startTime' must be before 'endTime'.");
        }

        var job = JobMapper.ToEntity(request);
        job.IdCompany = companyId; // authoritative source: JWT, never the request body
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
            Deliverables = string.IsNullOrWhiteSpace(job.Deliverables)
                ? null
                : JsonSerializer.Deserialize<List<string>>(job.Deliverables),
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
        if (request.Deliverables != null) job.Deliverables = JsonSerializer.Serialize(request.Deliverables);

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
            Deliverables = string.IsNullOrWhiteSpace(updated.Deliverables)
                ? null
                : JsonSerializer.Deserialize<List<string>>(updated.Deliverables),
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

    public async Task CancelJobAsync(int jobId, Guid companyId)
    {
        var job = await _jobRepository.GetByIdWithCompanyAsync(jobId);
        if (job == null)
            throw new KeyNotFoundException("Job not found");

        if (job.IdCompany != companyId)
            throw new UnauthorizedAccessException("Company does not own this job");

        if (job.Status == "cancelled")
            throw new InvalidOperationException("Job is already cancelled");

        var hasActiveContract = await _jobRepository.HasActiveContractAsync(jobId);
        if (hasActiveContract)
            throw new InvalidOperationException("Cannot cancel a job with an active contract");

        var applicantIds = await _jobRepository.GetApplicantIdsByJobIdAsync(jobId);

        job.Status = "cancelled";
        job.UpdatedAt = DateTime.UtcNow;
        await _jobRepository.CancelAsync(job);

        if (applicantIds.Count > 0)
        {
            var notifications = applicantIds.Select(studentId => new Notification
            {
                IdUser = studentId,
                Title = "Oferta cancelada",
                Body = $"La oferta \"{job.Title}\" ha sido cancelada por la empresa.",
                Type = "job_cancelled",
                Data = $"{{\"jobId\": {jobId}}}"
            }).ToList();

            await _notificationRepository.CreateManyAsync(notifications);
        }
    }
}