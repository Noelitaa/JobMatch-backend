using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<RatingResponse> CreateRatingAsync(CreateRatingRequest request, Guid callerId)
    {
        if (request.Stars <= 2 && string.IsNullOrWhiteSpace(request.Comment))
            throw new InvalidOperationException("El comentario es obligatorio cuando la calificación es de 1 o 2 estrellas.");

        var contract = await _ratingRepository.GetContractByIdAsync(request.IdContract);
        if (contract == null)
            throw new KeyNotFoundException("Contrato no encontrado.");

        var isStudent = contract.IdStudent == callerId;
        var isCompany = contract.IdCompany == callerId;

        if (!isStudent && !isCompany)
            throw new UnauthorizedAccessException("No tienes permiso para calificar este contrato.");

        if (!string.Equals(contract.Status, "active", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo se pueden calificar contratos activos.");

        var alreadyRated = await _ratingRepository.HasRatedAsync(request.IdContract, callerId);
        if (alreadyRated)
            throw new InvalidOperationException("Ya calificaste este contrato.");

        var idRated = isStudent ? contract.IdCompany : contract.IdStudent;

        var rating = new Rating
        {
            IdContract = request.IdContract,
            IdRater = callerId,
            IdRated = idRated,
            Stars = request.Stars,
            Comment = request.Comment
        };

        var created = await _ratingRepository.AddAsync(rating);

        return new RatingResponse
        {
            IdRating = created.IdRating,
            IdContract = created.IdContract,
            IdRated = created.IdRated,
            Stars = created.Stars,
            Comment = created.Comment,
            CreatedAt = created.CreatedAt
        };
    }
}
