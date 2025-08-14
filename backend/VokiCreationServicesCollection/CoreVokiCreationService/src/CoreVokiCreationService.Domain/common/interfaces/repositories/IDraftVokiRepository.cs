using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Domain.common.interfaces.repositories;

public interface IDraftVokiRepository
{
    Task Add(DraftVoki voki);
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId);
    Task<DraftVoki?> GetByIdAsNoTracking(VokiId vokiId);
    Task<DraftVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds);
    Task<DraftVoki?> GetById(VokiId vokiId);
    Task Update(DraftVoki voki);
    Task Delete(DraftVoki voki);
}