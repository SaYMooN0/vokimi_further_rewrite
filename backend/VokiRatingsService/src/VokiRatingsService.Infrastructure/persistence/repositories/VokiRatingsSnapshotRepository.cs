using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokiRatingsSnapshotRepository : IVokiRatingsSnapshotRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokiRatingsSnapshotRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public Task<VokiRatingsSnapshot?> GetLastSnapshotForVokiInThisDay(
        VokiId vokiId,
        DateOnly date,
        CancellationToken ct
    )
    {
        var dayStart = date.ToDateTime(TimeOnly.MinValue);
        var nextDayStart = dayStart.AddDays(1);

        return _db.VokiRatingsSnapshots
            .AsNoTracking()
            .Where(s =>
                s.VokiId == vokiId &&
                s.Date >= dayStart &&
                s.Date < nextDayStart
            )
            .OrderByDescending(s => s.Date)
            .FirstOrDefaultAsync(ct);
    }


    public async Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.VokiRatingsSnapshots.Add(snapshot);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.VokiRatingsSnapshots.Update(snapshot);
        await _db.SaveChangesAsync(ct);
    }
}