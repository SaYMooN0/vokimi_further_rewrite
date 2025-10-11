using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

public class RatingsRepository : IRatingsRepository
{
    private readonly VokiRatingsDbContext _db;

    public RatingsRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public Task<VokiRating?> GetByUserForVokiWithHistory(AppUserId userId, VokiId vokiId, CancellationToken ct) =>
        _db.Ratings
            .Include(r => r.History)
            .FirstOrDefaultAsync(r => r.VokiId == vokiId && r.UserId == userId, cancellationToken: ct);

    public Task<VokiRating[]> GetForVokiAsNoTracking(VokiId vokiId, CancellationToken ct) => _db.Ratings
        .AsNoTracking()
        .Where(r => r.VokiId == vokiId)
        .ToArrayAsync(cancellationToken: ct);

    public async Task Update(VokiRating rating, CancellationToken ct) {
        _db.Ratings.Update(rating);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Add(VokiRating rating, CancellationToken ct) {
        _db.Ratings.Add(rating);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiIdWithRatingDateDto[]> OrderedIdsOfVokiRatedByUser(AppUserId userId, CancellationToken ct) =>
        _db.Ratings
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.Current.DateTime)
            .Select(r => new VokiIdWithRatingDateDto(r.VokiId, r.Current.DateTime))
            .ToArrayAsync(ct);
}