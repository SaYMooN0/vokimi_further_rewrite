using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories.taking_sessions;

public interface IBaseTakingSessionsRepository
{
    public Task Add(BaseVokiTakingSession session);
}