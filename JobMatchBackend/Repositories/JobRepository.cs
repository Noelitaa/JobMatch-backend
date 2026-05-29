using Microsoft.EntityFrameworkCore;
using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _dbContext;

    public JobRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Job> CreateAsync(Job job)
    {
        _dbContext.Jobs.Add(job);
        await _dbContext.SaveChangesAsync();
        return job;
    }

    // De Develop: más eficiente, filtra IsActive en una sola query
    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _dbContext.Jobs
            .Include(j => j.Company)
            .Include(j => j.Applications)   // FIX: required for conflict check in DeleteJobAsync
            .FirstOrDefaultAsync(j => j.IdJob == id && j.Company != null && j.Company.IsActive);
    }

    public async Task<bool> IsCompanyOwnerAsync(int jobId, Guid companyId)
    {
        var job = await _dbContext.Jobs
            .FirstOrDefaultAsync(j => j.IdJob == jobId);
        return job != null && job.IdCompany == companyId;
    }

    public async Task<Job?> GetByIdWithCompanyAsync(int jobId)
    {
        var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.IdJob == jobId);
        if (job == null)
            return null;
        job.Company = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == job.IdCompany);
        if (job.Company == null || !job.Company.IsActive)
            return null;
        return job;
    }

    public async Task<List<Job>> GetAllAsync()
    {
        return await _dbContext.Jobs
            .Include(j => j.Company)
            .Where(j => j.Company != null && j.Company.IsActive)
            .ToListAsync();
    }

    // De feature/issue-23: funcionalidad nueva de borrado
    public async Task DeleteAsync(Job job)
    {
        _dbContext.Jobs.Remove(job);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Job job)
    {
        job.UpdatedAt = DateTime.UtcNow;
        _dbContext.Jobs.Update(job);
        await _dbContext.SaveChangesAsync();
    }

}