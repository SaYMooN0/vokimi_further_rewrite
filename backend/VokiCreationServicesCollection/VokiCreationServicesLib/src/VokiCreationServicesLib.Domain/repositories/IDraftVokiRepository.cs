using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Domain.repositories;

public interface IDraftVokiRepository
{
    Task<BaseDraftVoki?> GetByIdAsNoTracking(VokiId vokiId);
}