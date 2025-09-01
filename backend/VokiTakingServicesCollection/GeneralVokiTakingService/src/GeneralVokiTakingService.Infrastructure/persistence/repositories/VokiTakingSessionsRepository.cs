using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class VokiTakingSessionsRepository : IVokiTakingSessionsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public VokiTakingSessionsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public Task Add(BaseVokiTakingSession session) {
        _db.BaseVokiTakingSessions.Add(session);
        return _db.SaveChangesAsync();
    }

}