using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;

internal class BaseTakingSessionsRepository : IBaseTakingSessionsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public BaseTakingSessionsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task Add(BaseVokiTakingSession session, CancellationToken ct) {
        await _db.BaseVokiTakingSessions.AddAsync(session, ct);
        await _db.SaveChangesAsync(ct);
    }
}