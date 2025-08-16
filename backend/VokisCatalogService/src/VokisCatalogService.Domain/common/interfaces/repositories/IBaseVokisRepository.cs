using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Domain.common.interfaces.repositories;

public interface IBaseVokisRepository
{
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId);
    Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId);
    Task<BaseVoki[]> GetAllSortedAsNoTracking();
    Task<BaseVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds);
}