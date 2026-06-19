using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IRatingService
{
    Task<RatingResponse> CreateRatingAsync(CreateRatingRequest request, Guid callerId);
    Task<List<ReceivedRatingResponse>> GetMyRatingsAsync(Guid callerId);
}
