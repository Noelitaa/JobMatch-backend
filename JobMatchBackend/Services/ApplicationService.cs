// Services/ApplicationService.cs
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository; 


    public ApplicationService(IApplicationRepository applicationRepository,
    IJobRepository jobRepository)  
    {
        _applicationRepository = applicationRepository;
                _jobRepository = jobRepository;  

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

    public async Task<ApplicationResponse> GetApplicationDetailsAsync(int applicationId, Guid userId, string userRole)
    {
        var application = await _applicationRepository.GetApplicationWithDetailsAsync(applicationId);
        
        if (application == null)
            throw new KeyNotFoundException("Application not found");

        // Verificar permisos
        bool hasAccess = false;

        if (userRole == "Student")
        {
            hasAccess = application.IdStudent == userId;
        }
        else if (userRole == "Company")
        {
            var isOwner = await _jobRepository.IsCompanyOwnerAsync(application.IdJob, userId);
            hasAccess = isOwner;
        }
        else if (userRole == "Admin")
        {
            hasAccess = true;
        }

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to view this application");

        return new ApplicationResponse
        {
            IdApplication = application.IdApplication,
            IdJob = application.IdJob,
            JobTitle = application.Job?.Title ?? string.Empty,
            IdStudent = application.IdStudent,
            StudentName = application.Student?.FullName ?? string.Empty,
            StudentEmail = application.Student?.Email ?? string.Empty,
            StudentUniversity = application.Student?.University,
            StudentCareer = application.Student?.Career,
            Status = application.Status ?? "pending",
            CreatedAt = application.CreatedAt
        };
    }
}