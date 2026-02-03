using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.common.repositories.taking_sessions;

public interface IBaseTakingSessionsRepository
{
    public Task Add(BaseVokiTakingSession session, CancellationToken ct);
    Task<BaseVokiTakingSession?> GetForVokiAndUser(VokiId commandVokiId, AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task Delete(BaseVokiTakingSession session, CancellationToken ct);
}