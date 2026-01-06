using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using InfrastructureShared.EfCore.query_extensions;
using Microsoft.EntityFrameworkCore;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public DraftVokiRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }


    public async Task Add(DraftVoki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiId[]> ListVokiAuthoredByUserIdOrderByCreationDate(AppUserId userId, CancellationToken ct) =>
        _db.Vokis
            .FromSqlInterpolated($@"
                SELECT ""Id""
                FROM ""Vokis""
                WHERE {userId.Value} = ""PrimaryAuthorId""
                   OR {userId.Value} = ANY(""CoAuthorIds"")
                ORDER BY ""CreationDate"" DESC
            ")
            .Select(v => v.Id)
            .ToArrayAsync(cancellationToken: ct);

    public Task<DraftVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<DraftVoki[]> ListVokisWithUserAsInvitedForCoAuthorAsNoTracking(
        IAuthenticatedUserContext userContext, CancellationToken ct
    ) =>
        _db.Vokis
            .FromSqlInterpolated($@"
                SELECT *
                FROM ""Vokis""
                WHERE {userContext.UserId.Value} = ANY(""InvitedForCoAuthorUserIds"")
            ")
            .AsNoTracking()
            .ToArrayAsync(cancellationToken: ct);


    public Task<DraftVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds, CancellationToken ct) =>
        _db.Vokis.AsNoTracking()
            .Where(v => queryVokiIds.Contains(v.Id))
            .ToArrayAsync(cancellationToken: ct);

    public Task<DraftVoki?> GetById(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
        .ForUpdate()
        .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public async Task Update(DraftVoki voki, CancellationToken ct) {
        _db.Vokis.Update(voki);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Delete(DraftVoki voki, CancellationToken ct) {
        _db.Vokis.Remove(voki);
        await _db.SaveChangesAsync(ct);
    }
}