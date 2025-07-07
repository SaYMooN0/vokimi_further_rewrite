using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using Microsoft.EntityFrameworkCore;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public DraftVokiRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }

    public Task Add(DraftVoki voki) {
        _db.Vokis.Add(voki);
        return _db.SaveChangesAsync();
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId) =>
        _db.Vokis
            .FromSqlInterpolated($@"
                SELECT ""Id"", ""PrimaryAuthorId"", ""CoAuthorsIds"", ""CreationDate""
                FROM ""Vokis""
                WHERE {userId.Value} = ""PrimaryAuthorId""
                   OR {userId.Value} = ANY(""CoAuthorsIds"")
                ORDER BY ""CreationDate"" DESC
            ")
            .Select(v => v.Id)
            .ToArrayAsync();

    public Task<DraftVoki?> GetByIdAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<DraftVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds) => 
        _db.Vokis.AsNoTracking()
            .Where(v=> queryVokiIds.Contains(v.Id))
            .ToArrayAsync();
}