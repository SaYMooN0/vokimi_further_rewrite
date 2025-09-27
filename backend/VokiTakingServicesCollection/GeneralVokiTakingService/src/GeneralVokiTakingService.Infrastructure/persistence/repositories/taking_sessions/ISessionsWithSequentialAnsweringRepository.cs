using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

public class SessionsWithSequentialAnsweringRepository : ISessionsWithSequentialAnsweringRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public SessionsWithSequentialAnsweringRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task<SessionWithSequentialAnswering?> GetById(VokiTakingSessionId sessionId) =>
        await _db.VokiTakingSessionsWithSequentialAnswering.FindAsync(sessionId);

    public async Task Delete(SessionWithSequentialAnswering question) {
        _db.VokiTakingSessionsWithSequentialAnswering.Remove(question);
        await _db.SaveChangesAsync();
    }
}