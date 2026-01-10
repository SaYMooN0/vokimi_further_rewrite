using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.query_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class BaseVokisRepository : IBaseVokisRepository
{
    private readonly VokisCatalogDbContext _db;

    public BaseVokisRepository(VokisCatalogDbContext db) {
        _db = db;
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(
        AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct
    ) => _db.Database
        .SqlQuery<Guid>($@"
            SELECT ""Id"" AS ""Value""
            FROM ""BaseVokis""
            WHERE ""PrimaryAuthorId"" = {authenticatedUserCtx.UserId.Value}
               OR {authenticatedUserCtx.UserId.Value} = ANY(""CoAuthorIds"")
            ORDER BY ""PublicationDate"" DESC
        ")
        .Select(id => new VokiId(id))
        .ToArrayAsync(cancellationToken: ct);


    public Task<BaseVoki?> GetById(VokiId vokiId, CancellationToken ct) =>
        _db.BaseVokis
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<BaseVoki[]> GetAllSorted(CancellationToken ct) => _db.BaseVokis
        .OrderByDescending(v => v.PublicationDate)
        .ToArrayAsync(cancellationToken: ct);

    public Task<BaseVoki[]> GetMultipleById(VokiId[] queryVokiIds, CancellationToken ct) =>
        _db.BaseVokis
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync(cancellationToken: ct);

    public async Task<BaseVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct) =>
        await _db.BaseVokis
            .ForUpdate()
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public async Task Update(BaseVoki voki, CancellationToken ct) {
        _db.ThrowIfDetached(voki);
        _db.BaseVokis.Update(voki);
        await _db.SaveChangesAsync(ct);
    }
}