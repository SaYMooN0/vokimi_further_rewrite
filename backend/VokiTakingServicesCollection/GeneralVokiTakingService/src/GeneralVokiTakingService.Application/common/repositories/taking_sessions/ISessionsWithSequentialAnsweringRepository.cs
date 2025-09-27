using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories.taking_sessions;

public interface ISessionsWithSequentialAnsweringRepository
{
    Task<SessionWithSequentialAnswering?> GetById(VokiTakingSessionId sessionId);
    Task Delete(SessionWithSequentialAnswering question);

}