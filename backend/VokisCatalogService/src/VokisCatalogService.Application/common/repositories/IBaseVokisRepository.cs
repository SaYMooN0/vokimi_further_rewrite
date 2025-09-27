using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.common.repositories;

public interface IBaseVokisRepository
{
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId);
    Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId);
    Task<BaseVoki[]> GetAllSortedAsNoTracking();
    Task<BaseVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds);
    Task<BaseVoki?> GetById(VokiId vokiId);
    Task Update(BaseVoki voki);
}