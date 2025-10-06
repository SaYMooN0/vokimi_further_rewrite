using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IRatingsRepository
{
    Task<VokiRating?> GetByUserForVokiWithHistory(AppUserId userId, VokiId vokiId, CancellationToken ct);
    Task<VokiRating[]> GetForVokiAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task Update(VokiRating rating, CancellationToken ct);
    Task Add(VokiRating rating, CancellationToken ct);
    Task<VokiIdWithRatingDateDto[]> OrderedIdsOfVokiRatedByUser(AppUserId userId, CancellationToken ct);
}
public record VokiIdWithRatingDateDto(VokiId VokiId, DateTime Date);
