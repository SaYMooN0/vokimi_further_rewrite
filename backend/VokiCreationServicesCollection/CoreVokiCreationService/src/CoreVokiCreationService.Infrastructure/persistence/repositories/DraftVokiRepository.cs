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
            .Where(v => v.PrimaryAuthorId == userId || v.CoAuthorsIds.Contains(userId))
            .Select(s => s.Id)
            .ToArrayAsync();
}