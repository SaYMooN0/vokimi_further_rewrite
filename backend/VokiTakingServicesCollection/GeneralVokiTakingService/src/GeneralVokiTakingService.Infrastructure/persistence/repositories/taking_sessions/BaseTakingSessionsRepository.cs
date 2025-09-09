using GeneralVokiTakingService.Domain.common.interfaces.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

internal class BaseTakingSessionsRepository : IBaseTakingSessionsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public BaseTakingSessionsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public Task Add(BaseVokiTakingSession session) {
        _db.BaseVokiTakingSessions.Add(session);
        return _db.SaveChangesAsync();
    }
}