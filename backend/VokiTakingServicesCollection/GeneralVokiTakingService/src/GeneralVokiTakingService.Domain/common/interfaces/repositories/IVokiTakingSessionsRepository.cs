using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IVokiTakingSessionsRepository
{
    public Task Add(BaseVokiTakingSession session);
}