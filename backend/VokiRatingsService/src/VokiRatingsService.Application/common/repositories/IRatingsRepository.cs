using SharedKernel;
using SharedKernel.user_ctx;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_rating_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application.common.repositories;

public interface IRatingsRepository
{
    Task<VokiRating?> GetUserRatingForVoki(AuthenticatedUserCtx userContext, VokiId vokiId, CancellationToken ct);
    Task<VokiRating[]> GetForVokiAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task Update(VokiRating rating, CancellationToken ct);
    Task Add(VokiRating rating, CancellationToken ct);
    Task<VokiIdWithCurrentRatingDto[]> ListIdsOfVokiRatedByUser(AuthenticatedUserCtx userContext, CancellationToken ct);
    Task<VokiRatingsDistribution> GetRatingsDistributionForVoki(VokiId vokiId, CancellationToken ct);
}

public record VokiIdWithCurrentRatingDto(VokiId VokiId, RatingValue Value, DateTime DateTime);