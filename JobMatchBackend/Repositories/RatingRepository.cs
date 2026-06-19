using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly AppDbContext _dbContext;

    public RatingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Contract?> GetContractByIdAsync(int contractId)
    {
        return await _dbContext.Contracts
            .FirstOrDefaultAsync(c => c.IdContract == contractId);
    }

    public async Task<bool> HasRatedAsync(int contractId, Guid raterId)
    {
        return await _dbContext.Ratings
            .AnyAsync(r => r.IdContract == contractId && r.IdRater == raterId);
    }

    public async Task<Rating> AddAsync(Rating rating)
    {
        _dbContext.Ratings.Add(rating);
        await _dbContext.SaveChangesAsync();
        return rating;
    }

    public async Task<float> GetAverageRatingAsync(Guid userId)
    {
        var ratings = await _dbContext.Ratings
            .Where(r => r.IdRated == userId)
            .ToListAsync();

        if (ratings.Count == 0) return 0.0f;

        return (float)ratings.Average(r => r.Stars);
    }
}
