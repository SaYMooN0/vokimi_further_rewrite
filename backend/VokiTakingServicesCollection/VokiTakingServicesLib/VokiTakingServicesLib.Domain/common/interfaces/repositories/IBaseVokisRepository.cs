using SharedKernel.domain.ids;
using VokiTakingServicesLib.Domain.general_voki_aggregate;

namespace VokiTakingServicesLib.Domain.common.interfaces.repositories;

public interface IBaseVokisRepository
{
    Task<BaseVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct);
}