// Services/ApplicationService.cs
using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationService(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsByJobAsync(int jobId, int companyId)
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
        int applicationId, int companyId, UpdateApplicationRequest request)
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
}