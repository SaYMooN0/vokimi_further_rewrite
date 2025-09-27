using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using Microsoft.EntityFrameworkCore;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public DraftVokiRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }


    public async Task Add(DraftVoki voki) {
        await _db.Vokis.AddAsync(voki);
        await _db.SaveChangesAsync();
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId) =>
        _db.Vokis
            .FromSqlInterpolated($@"
                SELECT ""Id""
                FROM ""Vokis""
                WHERE {userId.Value} = ""PrimaryAuthorId""
                   OR {userId.Value} = ANY(""CoAuthorIds"")
                ORDER BY ""CreationDate"" DESC
            ")
            .Select(v => v.Id)
            .ToArrayAsync();

    public Task<DraftVoki?> GetByIdAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<DraftVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds) =>
        _db.Vokis.AsNoTracking()
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync();

    public Task<DraftVoki?> GetById(VokiId vokiId) => _db.Vokis
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public async Task Update(DraftVoki voki) {
        _db.Vokis.Update(voki);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(DraftVoki voki) {
        _db.Vokis.Remove(voki);
        await _db.SaveChangesAsync();
    }
}