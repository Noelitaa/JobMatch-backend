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
            .FirstOrDefaultAsync(j => j.IdJob == id);
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
        return job;
    }

    public async Task<List<Job>> GetAllAsync()
    {
        return await _dbContext.Jobs.ToListAsync();
    }

    public async Task UpdateAsync(Job job)
    {
        job.UpdatedAt = DateTime.UtcNow;
        _dbContext.Jobs.Update(job);
        await _dbContext.SaveChangesAsync();
    }
}