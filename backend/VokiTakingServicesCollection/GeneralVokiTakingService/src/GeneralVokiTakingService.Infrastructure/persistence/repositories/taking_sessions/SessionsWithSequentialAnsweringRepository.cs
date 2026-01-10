using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.query_extensions;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

internal class SessionsWithSequentialAnsweringRepository : ISessionsWithSequentialAnsweringRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public SessionsWithSequentialAnsweringRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task<SessionWithSequentialAnswering?> GetByIdForUpdate(VokiTakingSessionId sessionId, CancellationToken ct) =>
        await _db.VokiTakingSessionsWithSequentialAnswering
            .ForUpdate()
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken: ct);

    public async Task Delete(SessionWithSequentialAnswering session, CancellationToken ct) {
        _db.ThrowIfDetached(session);
        _db.VokiTakingSessionsWithSequentialAnswering.Remove(session);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(SessionWithSequentialAnswering session, CancellationToken ct) {
        _db.ThrowIfDetached(session);
        _db.VokiTakingSessionsWithSequentialAnswering.Update(session);
        await _db.SaveChangesAsync(ct);
    }
}