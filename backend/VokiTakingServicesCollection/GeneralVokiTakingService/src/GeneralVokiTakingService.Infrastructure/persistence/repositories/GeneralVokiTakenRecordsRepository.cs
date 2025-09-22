using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class GeneralVokiTakenRecordsRepository : IGeneralVokiTakenRecordsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public GeneralVokiTakenRecordsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralVokiTakenRecord vokiTakenRecord) {
        await _db.VokiTakenRecords.AddAsync(vokiTakenRecord);
        await _db.SaveChangesAsync();
    }

    public Task<GeneralVokiTakenRecord[]> ForVokiByUserAsNoTracking(
        VokiId vokiId, AppUserId userId, CancellationToken ct
    ) => _db.VokiTakenRecords
        .AsNoTracking()
        .Where(r => r.TakenVokiId == vokiId && r.VokiTakerId == userId)
        .ToArrayAsync(ct);

    public async Task<Dictionary<GeneralVokiResultId, uint>> GetResultIdsToCountForVoki(
        VokiId vokiId, CancellationToken ct
    ) => await _db.VokiTakenRecords
        .AsNoTracking()
        .Where(r => r.TakenVokiId == vokiId)
        .GroupBy(r => r.ReceivedResultId)
        .Select(g => new { ResultId = g.Key, Count = g.Count() })
        .ToDictionaryAsync(x => x.ResultId, x => (uint)x.Count, ct);
}