using Microsoft.EntityFrameworkCore;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class BaseVokisRepository : IBaseVokisRepository
{
    private readonly VokisCatalogDbContext _db;

    public BaseVokisRepository(VokisCatalogDbContext db) {
        _db = db;
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId, CancellationToken ct) => _db.Database
        .SqlQuery<Guid>($@"
            SELECT ""Id"" AS ""Value""
            FROM ""BaseVokis""
            WHERE ""PrimaryAuthorId"" = {userId.Value}
               OR {userId.Value} = ANY(""CoAuthorIds"")
            ORDER BY ""PublicationDate"" DESC
        ")
        .Select(id => new VokiId(id))
        .ToArrayAsync(cancellationToken: ct);


    public Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        _db.BaseVokis
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<BaseVoki[]> GetAllSortedAsNoTracking(CancellationToken ct) => _db.BaseVokis
        .AsNoTracking()
        .OrderByDescending(v => v.PublicationDate)
        .ToArrayAsync(cancellationToken: ct);

    public Task<BaseVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds, CancellationToken ct) =>
        _db.BaseVokis.AsNoTracking()
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync(cancellationToken: ct);

    public async Task<BaseVoki?> GetById(VokiId vokiId) =>
        await _db.BaseVokis.FindAsync(vokiId);

    public async Task Update(BaseVoki voki, CancellationToken ct) {
        _db.BaseVokis.Update(voki);
        await _db.SaveChangesAsync(ct);
    }
}