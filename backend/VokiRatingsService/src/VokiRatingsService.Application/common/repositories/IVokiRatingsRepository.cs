using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokiRatingsRepository
{
    public Task<VokiRating?> GetByUserForVoki(AppUserId userId, VokiId vokiId);
    Task Update(VokiRating rating);
    Task Add(VokiRating rating);
    Task<(uint RatingsSum, uint RatingsCount)> GetRatingsSumAndCountForVoki(VokiId commandVokiId);
}