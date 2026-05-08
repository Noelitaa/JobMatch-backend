using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _dbContext;

    public JobRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Job?> GetByIdWithCompanyAsync(int jobId)
    {
        var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.IdJob == jobId);
        if (job == null) return null;

        job.Company = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == job.IdCompany);
        return job;
    }

    public async Task<Job?> GetByIdAsync(int jobId)
    {
        return await _dbContext.Jobs.FirstOrDefaultAsync(j => j.IdJob == jobId);
    }

    public async Task<bool> HasAcceptedApplicationsAsync(int jobId)
    {
        return await _dbContext.Applications
            .AnyAsync(a => a.IdJob == jobId && a.Status == "accepted");
    }

    public async Task<Job> UpdateAsync(Job job)
    {
        job.UpdatedAt = DateTime.UtcNow;
        _dbContext.Jobs.Update(job);
        await _dbContext.SaveChangesAsync();
        return job;
    }
}