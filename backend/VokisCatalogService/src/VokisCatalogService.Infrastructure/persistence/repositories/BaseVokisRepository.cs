using Microsoft.EntityFrameworkCore;
using VokisCatalogService.Domain.common.interfaces.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class BaseVokisRepository : IBaseVokisRepository
{
    private readonly VokisCatalogTakingDbContext _db;

    public BaseVokisRepository(VokisCatalogTakingDbContext db) {
        _db = db;
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId) => _db.Database
        .SqlQuery<Guid>($@"
            SELECT ""Id"" AS ""Value""
            FROM ""BaseVokis""
            WHERE ""PrimaryAuthorId"" = {userId.Value}
               OR {userId.Value} = ANY(""CoAuthorIds"")
            ORDER BY ""PublicationDate"" DESC
        ")
        .Select(id => new VokiId(id))
        .ToArrayAsync();


    public Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId) =>
        _db.BaseVokis
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<BaseVoki[]> GetAllSortedAsNoTracking() => _db.BaseVokis
        .AsNoTracking()
        .OrderByDescending(v => v.PublicationDate)
        .ToArrayAsync();

    public Task<BaseVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds) =>
        _db.BaseVokis.AsNoTracking()
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync();

    public async Task<BaseVoki?> GetById(VokiId vokiId) =>
        await _db.BaseVokis.FindAsync(vokiId);

    public async Task Update(BaseVoki voki) {
        _db.BaseVokis.Update(voki);
        await _db.SaveChangesAsync();
    }
}