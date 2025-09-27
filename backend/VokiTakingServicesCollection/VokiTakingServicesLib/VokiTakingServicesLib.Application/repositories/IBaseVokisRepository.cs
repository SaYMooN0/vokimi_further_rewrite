using SharedKernel.domain.ids;
using VokiTakingServicesLib.Domain.base_voki_aggregate;

namespace VokiTakingServicesLib.Application.repositories;

public interface IBaseVokisRepository
{
    Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct);
}