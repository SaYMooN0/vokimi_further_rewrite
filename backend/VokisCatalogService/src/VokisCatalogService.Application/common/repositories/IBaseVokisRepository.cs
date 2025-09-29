using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.common.repositories;

public interface IBaseVokisRepository
{
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AppUserId userId, CancellationToken ct);
    Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<BaseVoki[]> GetAllSortedAsNoTracking(CancellationToken ct);
    Task<BaseVoki[]> GetMultipleByIdAsNoTracking(VokiId[] queryVokiIds, CancellationToken ct);
    Task<BaseVoki?> GetById(VokiId vokiId);
    Task Update(BaseVoki voki, CancellationToken ct = default);
}