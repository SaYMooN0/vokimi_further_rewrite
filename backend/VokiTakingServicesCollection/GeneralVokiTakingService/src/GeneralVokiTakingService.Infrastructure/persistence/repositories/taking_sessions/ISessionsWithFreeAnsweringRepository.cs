using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.interfaces.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

public class SessionsWithFreeAnsweringRepository : ISessionsWithFreeAnsweringRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public SessionsWithFreeAnsweringRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task<SessionWithFreeAnswering?> GetById(VokiTakingSessionId sessionId) =>
        await _db.VokiTakingSessionsWithFreeAnswering.FindAsync(sessionId);
}