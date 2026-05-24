using System.Text.Json;
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IContractRepository _contractRepository;

    public ApplicationService(
        IApplicationRepository applicationRepository,
        IJobRepository jobRepository,
        IContractRepository contractRepository)
    {
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
        _contractRepository = contractRepository;
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsByJobAsync(int jobId, Guid companyId)
    {
        var isOwner = await _applicationRepository.IsCompanyOwnerAsync(jobId, companyId);
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

        var isOwner = await _applicationRepository.IsCompanyOwnerAsync(application.IdJob, companyId);
        var isOwner = await _jobRepository.IsCompanyOwnerAsync(application.IdJob, companyId);
        if (!isOwner)
            throw new UnauthorizedAccessException("Company does not own this job opening");

        if (application.Status != "pending")
            throw new InvalidOperationException($"Application is already {application.Status}");

        if (request.Status != "accepted" && request.Status != "rejected")
            throw new ArgumentException("Invalid status. Use 'accepted' or 'rejected'");

        application.Status = request.Status;
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
                Status = contract.Status ?? "pending",
                CreatedAt = contract.CreatedAt,
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
            throw new InvalidOperationException($"No se pudieron obtener los datos para la aplicación {application.IdApplication}");

        var job = await _jobRepository.GetByIdAsync(application.IdJob);

        if (job == null)
            throw new InvalidOperationException($"No se encontró el trabajo con ID {application.IdJob}");

        var contractData = new
        {
            jobTitle = data.JobTitle,
            companyName = data.CompanyName ?? data.CompanyOwnerName,
            companyEmail = data.CompanyEmail,
            studentName = data.StudentName,
            studentEmail = data.StudentEmail,
            studentUniversity = data.StudentUniversity,
            studentCareer = data.StudentCareer,
            workType = data.JobType ?? "No especificado",
            startDate = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd"),
            endDate = DateTime.UtcNow.AddMonths(3).ToString("yyyy-MM-dd"),
            compensation = "Por definir según acuerdo",
            clauses = new[]
            {
                "El estudiante se compromete a cumplir con las tareas asignadas",
                "La empresa proporcionará los recursos necesarios",
                "Se respetarán los horarios acordados",
                "Cualquier modificación debe ser acordada por escrito",
                "El incumplimiento puede resultar en terminación del contrato"
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
            ApplicationId = application.IdApplication,
            Status = application.Status ?? "pending",
            JobId = application.IdJob,
            StudentId = application.IdStudent,
            CreatedAt = application.CreatedAt
        };
    }
}
