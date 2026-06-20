using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IRatingRepository
{
    Task<Contract?> GetContractByIdAsync(int contractId);
    Task<bool> HasRatedAsync(int contractId, Guid raterId);
    Task<Rating> AddAsync(Rating rating);
    Task<float> GetAverageRatingAsync(Guid userId);
    Task<List<Rating>> GetReceivedRatingsAsync(Guid userId);
}
