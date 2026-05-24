using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.DTOs;

namespace JobMatchBackend.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly AppDbContext _dbContext;

    public ApplicationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application?> GetByIdAsync(int id)
    {
        var application = await _dbContext.Applications
            .FirstOrDefaultAsync(a => a.IdApplication == id);

        if (application == null)
            return null;

        if (application.IdJob > 0)
        {
            application.Job = await _dbContext.Jobs
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.IdJob == application.IdJob);
        }

        if (application.IdStudent != Guid.Empty)
        {
            application.Student = await _dbContext.User
                .FirstOrDefaultAsync(u => u.Id == application.IdStudent);
        }

        return application;
    }

    public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
    {
        var applications = await _dbContext.Applications
            .Where(a => a.IdJob == jobId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        foreach (var app in applications)
        {
            if (app.IdJob > 0)
            {
                app.Job = await _dbContext.Jobs
                    .Include(j => j.Company)
                    .FirstOrDefaultAsync(j => j.IdJob == app.IdJob);
            }

            if (app.IdStudent != Guid.Empty)
            {
                app.Student = await _dbContext.User
                    .FirstOrDefaultAsync(u => u.Id == app.IdStudent);
            }
        }

        return applications;
    }

    public async Task UpdateAsync(Application application)
    {
        application.UpdatedAt = DateTime.UtcNow;
        _dbContext.Applications.Update(application);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId)
    {
        var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.IdJob == jobId);
        return job != null && job.IdCompany == companyId; 
    }

    public async Task<Application?> GetApplicationWithDetailsAsync(int applicationId)
    {
        return await _dbContext.Applications
            .Include(a => a.Student)
            .Include(a => a.Job)
                .ThenInclude(j => j!.Company)
            .FirstOrDefaultAsync(a => a.IdApplication == applicationId);
public async Task<ContractDataDto?> GetContractDataAsync(int applicationId)
{
    try
    {
        var query = $@"
            SELECT 
                a.IdApplication,
                a.IdJob,
                a.IdStudent,
                j.Title as JobTitle,
                j.Type as JobType,
                u.CompanyName,
                u.Email as CompanyEmail,
                u.FullName as CompanyOwnerName,
                s.FullName as StudentName,
                s.Email as StudentEmail,
                s.University as StudentUniversity,
                s.Career as StudentCareer
            FROM Applications a
            INNER JOIN Jobs j ON a.IdJob = j.IdJob
            INNER JOIN [User] u ON j.IdCompany = u.Id
            INNER JOIN [User] s ON a.IdStudent = s.Id
            WHERE a.IdApplication = {applicationId}";

        var result = await _dbContext.ContractData
            .FromSqlRaw(query)
            .FirstOrDefaultAsync();
        
        return result;
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException($"Error en GetContractDataAsync: {ex.Message}");
    }
}
