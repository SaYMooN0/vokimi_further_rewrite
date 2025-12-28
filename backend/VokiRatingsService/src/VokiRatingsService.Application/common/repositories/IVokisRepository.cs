using SharedKernel.common.vokis;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokisRepository
{
    Task<Voki?> GetById(VokiId vokiId, CancellationToken ct);
    Task Update(Voki voki, CancellationToken ct);
    Task Add(Voki voki, CancellationToken ct);
}
public record VokiManagersDto(VokiId Id, AppUserId PrimaryAuthorId, VokiManagersIdsSet ManagersIds);