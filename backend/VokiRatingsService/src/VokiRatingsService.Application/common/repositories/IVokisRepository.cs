using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokisRepository
{
    Task<Voki?> GetById(VokiId vokiId, CancellationToken ct);
    Task Update(Voki voki, CancellationToken ct);
    Task Add(Voki voki, CancellationToken ct);
}