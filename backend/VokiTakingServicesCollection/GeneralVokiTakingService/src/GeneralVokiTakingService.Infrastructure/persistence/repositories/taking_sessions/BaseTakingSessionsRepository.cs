using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

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

    public async Task Delete(BaseVokiTakingSession session, CancellationToken ct) {
        _db.ThrowIfDetached(session);
        _db.BaseVokiTakingSessions.Remove(session);
        await _db.SaveChangesAsync(ct);
    }

    public Task<BaseVokiTakingSession?> GetForVokiAndUser(
        VokiId commandVokiId, AuthenticatedUserCtx aUserCtx, CancellationToken ct
    ) {
        return _db.BaseVokiTakingSessions
            .FirstOrDefaultAsync(s => s.VokiId == commandVokiId && s.VokiTaker == aUserCtx.UserId, ct);
    }
}