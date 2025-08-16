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

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId) =>
        _db.BaseVokis
            .FromSqlInterpolated($@"
                SELECT ""Id"", ""PrimaryAuthorId"", ""CoAuthorIds"", ""CreationDate""
                FROM ""Vokis""
                WHERE {userId.Value} = ""PrimaryAuthorId""
                   OR {userId.Value} = ANY(""CoAuthorIds"")
                ORDER BY ""CreationDate"" DESC
            ")
            .Select(v => v.Id)
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
}