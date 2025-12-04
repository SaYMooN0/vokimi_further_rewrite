using SharedKernel;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IRatingsRepository
{
    Task<VokiRating?> GetByUserForVokiWithHistory(AppUserId userId, VokiId vokiId, CancellationToken ct);
    Task<VokiRating[]> GetForVokiAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task Update(VokiRating rating, CancellationToken ct);
    Task Add(VokiRating rating, CancellationToken ct);

    Task<VokiIdWithLastRatingDto[]> OrderedIdsOfVokiRatedByUser(
        IAuthenticatedUserContext userContext, CancellationToken ct
    );
}

public record VokiIdWithLastRatingDto(VokiId VokiId, ushort Value, DateTime DateTime);