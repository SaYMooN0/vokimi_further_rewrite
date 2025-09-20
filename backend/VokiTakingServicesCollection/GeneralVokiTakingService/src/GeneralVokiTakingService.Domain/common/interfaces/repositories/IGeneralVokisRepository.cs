using GeneralVokiTakingService.Domain.general_voki_aggregate;
using VokiTakingServicesLib.Domain.common.interfaces.repositories;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IGeneralVokisRepository : IBaseVokisRepository
{
    Task Add(GeneralVoki voki, CancellationToken ct = default);
    Task<GeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithResultsByIdAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetById(VokiId vokiId, CancellationToken ct);
    Task Update(GeneralVoki voki, CancellationToken ct);
}