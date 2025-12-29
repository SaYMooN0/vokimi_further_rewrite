using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

internal class SessionsWithFreeAnsweringRepository : ISessionsWithFreeAnsweringRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public SessionsWithFreeAnsweringRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task<SessionWithFreeAnswering?> GetById(VokiTakingSessionId sessionId, CancellationToken ct) =>
        await _db.VokiTakingSessionsWithFreeAnswering.FindAsync([sessionId], cancellationToken: ct);

    public async Task Delete(SessionWithFreeAnswering question , CancellationToken ct) {
        _db.BaseVokiTakingSessions.Remove(question);
        await _db.SaveChangesAsync(ct);
    }
}