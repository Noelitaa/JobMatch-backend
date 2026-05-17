// Repositories/ApplicationRepository.cs
using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;

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
        return await _dbContext.Applications
            .Include(a => a.Student)
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.IdApplication == id);
    }

    public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
    {
        return await _dbContext.Applications
            .Include(a => a.Student)
            .Include(a => a.Job)
            .Where(a => a.IdJob == jobId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Application application)
    {
        application.UpdatedAt = DateTime.UtcNow;
        _dbContext.Applications.Update(application);
        await _dbContext.SaveChangesAsync();
    }

   // Repositories/ApplicationRepository.cs
    public async Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId)
{
    var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.IdJob == jobId);
    return job != null && job.IdCompany == companyId;
}
}