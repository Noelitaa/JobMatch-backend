namespace JobMatchBackend.Repositories;

public interface IFcmTokenRepository
{
    Task SaveOrUpdateAsync(Guid userId, string token, string? deviceInfo);
    Task<List<string>> GetTokensByUserIdAsync(Guid userId);
    Task DeleteByTokenAsync(string token);
}
