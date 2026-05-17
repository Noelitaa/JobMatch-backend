// Services/ApplicationService.cs
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(
        IApplicationRepository applicationRepository,
        IUserRepository userRepository,
        IJobRepository jobRepository,
        ILogger<ApplicationService> logger)
    {
        _applicationRepository = applicationRepository;
        _userRepository = userRepository;
        _jobRepository = jobRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsByJobAsync(int jobId, Guid companyId)
    {
        // Verificar que la empresa es dueña
        var isOwner = await _applicationRepository.IsCompanyOwnerAsync(jobId, companyId);
        if (!isOwner)
            throw new UnauthorizedAccessException("Company does not own this job opening");

        var applications = await _applicationRepository.GetByJobIdAsync(jobId);
        
        return applications.Select(a => new ApplicationResponse
        {
            IdApplication = a.IdApplication,
            IdJob = a.IdJob,
            JobTitle = a.Job?.Title ?? string.Empty,
            IdStudent = a.IdStudent,
            StudentName = a.Student?.FullName ?? string.Empty,
            StudentEmail = a.Student?.Email ?? string.Empty,
            StudentUniversity = a.Student?.University,
            StudentCareer = a.Student?.Career,
            Status = a.Status ?? "pending",
            CreatedAt = a.CreatedAt
        });
    }

    public async Task<UpdateApplicationResponse> UpdateApplicationStatusAsync(
        int applicationId, Guid companyId, UpdateApplicationRequest request)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId);
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        // Verificar permisos
        var isOwner = await _applicationRepository.IsCompanyOwnerAsync(application.IdJob, companyId);
        if (!isOwner)
            throw new UnauthorizedAccessException("Company does not own this job opening");

        // Validar estado actual
        if (application.Status != "pending")
            throw new InvalidOperationException($"Application is already {application.Status}");

        // Validar status solicitado
        if (request.Status != "accepted" && request.Status != "rejected")
            throw new ArgumentException("Invalid status. Use 'accepted' or 'rejected'");

        // Actualizar estado
        application.Status = request.Status;
        await _applicationRepository.UpdateAsync(application);

        return new UpdateApplicationResponse
        {
            IdApplication = application.IdApplication,
            Status = application.Status,
            Message = $"Application {application.Status} successfully"
        };
    }

    public async Task<CreateApplicationResponse> CreateApplicationAsync(CreateApplicationRequest request)
    {
        var student = await _userRepository.GetByIdAsync(request.IdStudent!.Value);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var job = await _jobRepository.GetByIdWithCompanyAsync(request.IdJob!.Value);
        if (job == null)
            throw new KeyNotFoundException("Job offer not found");

        var alreadyApplied = await _applicationRepository.ExistsAsync(request.IdStudent.Value, request.IdJob.Value);
        if (alreadyApplied)
            throw new InvalidOperationException("Student has already applied to this job");

        var application = new Application
        {
            IdStudent = request.IdStudent.Value,
            IdJob = request.IdJob.Value,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await _applicationRepository.CreateAsync(application);

        NotifyCompany(job.Company?.Email, job.Title, student.FullName);

        return new CreateApplicationResponse
        {
            Message = "Application submitted successfully",
            Status = application.Status
        };
    }

    private void NotifyCompany(string? companyEmail, string jobTitle, string? studentName)
    {
        _logger.LogInformation(
            "NOTIFICATION: New application received — Job: '{JobTitle}', Applicant: '{StudentName}', Company email: '{CompanyEmail}'",
            jobTitle, studentName ?? "Unknown", companyEmail ?? "Unknown");
    }
}