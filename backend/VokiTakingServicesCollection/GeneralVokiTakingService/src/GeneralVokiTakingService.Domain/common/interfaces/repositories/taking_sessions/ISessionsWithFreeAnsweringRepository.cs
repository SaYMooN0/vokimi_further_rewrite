using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories.taking_sessions;

public interface ISessionsWithFreeAnsweringRepository
{
    Task<SessionWithFreeAnswering?> GetById(VokiTakingSessionId sessionId);
    Task Delete(SessionWithFreeAnswering question);
}