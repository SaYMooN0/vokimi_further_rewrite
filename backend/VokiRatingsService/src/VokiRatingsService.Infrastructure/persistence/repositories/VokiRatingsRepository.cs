using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

public class VokiRatingsRepository : IVokiRatingsRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokiRatingsRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public Task<VokiRating?> GetByUserForVoki(AppUserId userId, VokiId vokiId) =>
        _db.Ratings.FirstOrDefaultAsync(r => r.VokiId == vokiId && r.UserId == userId);

    public async Task Update(VokiRating rating) {
        _db.Ratings.Update(rating);
        await _db.SaveChangesAsync();
    }

    public async Task Add(VokiRating rating) {
        _db.Ratings.Add(rating);
        await _db.SaveChangesAsync();
    }

    public async Task<(uint RatingsSum, uint RatingsCount)> GetRatingsSumAndCountForVoki(VokiId commandVokiId) {
        var baseQuery = _db.Ratings
            .AsNoTracking()
            .Where(r => r.VokiId == commandVokiId);

        var count = await baseQuery.LongCountAsync();
        if (count == 0) {
            return (0u, 0u);
        }

        var sum = await baseQuery.SumAsync(r => r.Current.Value);

        return ((uint)sum, (uint)count);
    }
}