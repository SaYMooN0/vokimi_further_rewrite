using ApplicationShared;
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

    public Task<VokiIdWithLastRatingDto[]> ListIdsOfVokiRatedByUser(
        IAuthenticatedUserContext userContext, CancellationToken ct
    ) =>
        _db.Ratings
            .AsNoTracking()
            .Where(r => r.UserId == userContext.UserId)
            .Select(r => new VokiIdWithLastRatingDto(r.VokiId, r.Current.Value, r.Current.DateTime))
            .ToArrayAsync(ct);
}