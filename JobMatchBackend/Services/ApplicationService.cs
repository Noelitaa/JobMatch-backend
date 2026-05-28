using System.Text.Json;
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
    private readonly IContractRepository _contractRepository;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(
        IApplicationRepository applicationRepository,
        IUserRepository userRepository,
        IJobRepository jobRepository,
        IContractRepository contractRepository,
        ILogger<ApplicationService> logger)
    {
        _applicationRepository = applicationRepository;
        _userRepository = userRepository;
        _jobRepository = jobRepository;
        _contractRepository = contractRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsByJobAsync(int jobId, Guid companyId)
    {
        var isOwner = await _jobRepository.IsCompanyOwnerAsync(jobId, companyId);
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

        var isOwner = await _jobRepository.IsCompanyOwnerAsync(application.IdJob, companyId);
        if (!isOwner)
            throw new UnauthorizedAccessException("Company does not own this job opening");

        if (application.Status != "pending")
            throw new InvalidOperationException($"Application is already {application.Status}");

        if (request.Status != "accepted" && request.Status != "rejected")
            throw new ArgumentException("Invalid status. Use 'accepted' or 'rejected'");

        application.Status = request.Status;
        application.UpdatedAt = DateTime.UtcNow;
        await _applicationRepository.UpdateAsync(application);

        var response = new UpdateApplicationResponse
        {
            IdApplication = application.IdApplication,
            Status = application.Status,
            Message = $"Application {application.Status} successfully"
        };

        if (application.Status == "accepted")
        {
            var contract = await GenerateContractAsync(application);
            response.Contract = new ContractResponse
            {
                IdContract = contract.IdContract,
                IdApplication = contract.IdApplication,
                IdJob = contract.IdJob,
                IdStudent = contract.IdStudent,
                IdCompany = contract.IdCompany,
                Status = contract.Status ?? "pending",
                CreatedAt = contract.CreatedAt,
                UpdatedAt = contract.UpdatedAt,
                AcceptedAt = contract.AcceptedAt,
                ContractData = contract.ContractData
            };
            response.Message += " and contract generated";
        }

        return response;
    }

    private async Task<Contract> GenerateContractAsync(Application application)
    {
        var data = await _applicationRepository.GetContractDataAsync(application.IdApplication);

        if (data == null)

            throw new InvalidOperationException($"Could not retrieve contract data for application {application.IdApplication}");


        var job = await _jobRepository.GetByIdAsync(application.IdJob);
        if (job == null)

            throw new InvalidOperationException($"Job with ID {application.IdJob} not found");


        var contractData = new
        {
            jobTitle = data.JobTitle,
            companyName = data.CompanyName ?? data.CompanyOwnerName,
            companyEmail = data.CompanyEmail,
            studentName = data.StudentName,
            studentEmail = data.StudentEmail,
            studentUniversity = data.StudentUniversity,
            studentCareer = data.StudentCareer,
            workType = data.JobType ?? "Not specified",
            startDate = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd"),
            endDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
            compensation = "To be defined by agreement",
            clauses = new[]
            {
                "The student commits to complete assigned tasks.",
                "The company will provide the required resources.",
                "Agreed working schedules must be respected.",
                "Any modification must be agreed in writing.",
                "Non-compliance may result in contract termination."
            }
        };

        var contract = new Contract
        {
            IdApplication = application.IdApplication,
            IdJob = application.IdJob,
            IdStudent = application.IdStudent,
            IdCompany = job.IdCompany,
            Status = "pending",
            ContractData = JsonSerializer.Serialize(contractData),
            CreatedAt = DateTime.UtcNow
        };

        return await _contractRepository.CreateAsync(contract);
    }

    public async Task<CreateApplicationResponse> CreateApplicationAsync(Guid studentId, CreateApplicationRequest request)
    {
        if (request.IdJob == null)
            throw new ArgumentException("Job id is required");

        var student = await _userRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (student.Role != "Student")
            throw new UnauthorizedAccessException("Only students can apply for job offers");

        var job = await _jobRepository.GetByIdWithCompanyAsync(request.IdJob!.Value);
        if (job == null)
            throw new KeyNotFoundException("Job offer not found");

        var alreadyApplied = await _applicationRepository.ExistsAsync(studentId, request.IdJob.Value);
        if (alreadyApplied)
            throw new InvalidOperationException("Student has already applied to this job");

        var application = new Application
        {
            IdStudent = studentId,
            IdJob = request.IdJob.Value,
            Status = "pending"
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
            "NOTIFICATION: New application received - Job: '{JobTitle}', Applicant: '{StudentName}', Company email: '{CompanyEmail}'",
            jobTitle, studentName ?? "Unknown", companyEmail ?? "Unknown");
    }
    
    public async Task<ApplicationDetailResponse> GetApplicationDetailsAsync(int applicationId, Guid userId, string userRole)
    {
        var application = await _applicationRepository.GetApplicationWithDetailsAsync(applicationId);

        if (application == null)
            throw new KeyNotFoundException("Application not found");

        var hasAccess =
            userRole == "Admin" ||
            (userRole == "Student" && application.IdStudent == userId) ||
            (userRole == "Company" && application.Job?.IdCompany == userId);

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to view this application");

        return new ApplicationDetailResponse
        {
            IdApplication = application.IdApplication,
            Status = application.Status ?? "pending",
            IdJob = application.IdJob,
            JobTitle = application.Job?.Title ?? string.Empty,
            JobType = application.Job?.Type,
            IdCompany = application.Job?.IdCompany ?? Guid.Empty,
            CompanyName = application.Job?.Company?.CompanyName,
            IdStudent = application.IdStudent,
            StudentName = application.Student?.FullName,
            StudentEmail = application.Student?.Email,
            StudentUniversity = application.Student?.University,
            StudentCareer = application.Student?.Career,
            CreatedAt = application.CreatedAt
        };
    }
}
