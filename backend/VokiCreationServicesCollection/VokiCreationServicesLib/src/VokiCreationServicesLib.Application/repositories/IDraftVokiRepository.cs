using SharedKernel.domain.ids;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.repositories;

public interface IDraftVokiRepository
{
    Task<BaseDraftVoki?> GetByIdAsNoTracking(VokiId vokiId);
}