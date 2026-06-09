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

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _dbContext.Jobs
            .Include(j => j.Company)
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

    public async Task<Job> UpdateAsync(Job job)
    {
        _dbContext.Jobs.Update(job);
        await _dbContext.SaveChangesAsync();
        return job;
    }

    public async Task<bool> HasAcceptedApplicationsAsync(int jobId)
    {
        return await _dbContext.Applications
            .AnyAsync(a => a.IdJob == jobId && a.Status == "accepted");
    }

    public async Task<bool> HasActiveContractAsync(int jobId)
    {
        return await _dbContext.Contracts
            .AnyAsync(c => c.IdJob == jobId && c.Status == "active");
    }
}
