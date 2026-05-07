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
        if (job == null)
        {
            return null;
        }

        job.Company = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == job.IdCompany);
        return job;
    }

    public async Task<List<Job>> GetAllAsync()
    {
        return await _context.Jobs.ToListAsync();
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _context.Jobs.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var Job = await _context.Jobs.FindAsync(id);
        if (Job == null) return false;

        _context.Jobs.Remove(Job);
        await _context.SaveChangesAsync();
        return true;
    }
}
