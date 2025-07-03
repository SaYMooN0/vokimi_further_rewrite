using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Domain.common.interfaces.repositories;

public interface IDraftVokiRepository
{
    Task Add(DraftVoki voki);
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId);
}