using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokiRatingsRepository
{
    public Task<VokiRating?> GetByUserForVokiWithHistory(AppUserId userId, VokiId vokiId, CancellationToken ct);
    public Task<VokiRating[]> GetForVokiAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task Update(VokiRating rating, CancellationToken ct);
    Task Add(VokiRating rating, CancellationToken ct);
}