using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class FcmTokenRepository : IFcmTokenRepository
{
    private readonly AppDbContext _dbContext;

    public FcmTokenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveOrUpdateAsync(Guid userId, string token, string? deviceInfo)
    {
        var existing = await _dbContext.FcmTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (existing != null)
        {
            existing.IdUser = userId;
            existing.DeviceInfo = deviceInfo;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _dbContext.FcmTokens.Add(new FcmToken
            {
                IdUser = userId,
                Token = token,
                DeviceInfo = deviceInfo
            });
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetTokensByUserIdAsync(Guid userId)
    {
        return await _dbContext.FcmTokens
            .Where(t => t.IdUser == userId)
            .Select(t => t.Token)
            .ToListAsync();
    }

    public async Task DeleteByTokenAsync(string token)
    {
        var entity = await _dbContext.FcmTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (entity != null)
        {
            _dbContext.FcmTokens.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
