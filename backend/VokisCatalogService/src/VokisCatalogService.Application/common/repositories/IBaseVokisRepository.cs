using SharedKernel.user_ctx;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.common.repositories;

public interface IBaseVokisRepository
{
    Task<VokiId[]> ListVokiAuthoredByUserIdsOrderByCreationDate(AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct);
    Task<BaseVoki?> GetById(VokiId vokiId, CancellationToken ct);
    Task<BaseVoki[]> GetAllSorted(CancellationToken ct);
    Task<BaseVoki[]> GetMultipleById(VokiId[] queryVokiIds, CancellationToken ct);
    Task<BaseVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct);
    Task Update(BaseVoki voki, CancellationToken ct);
}