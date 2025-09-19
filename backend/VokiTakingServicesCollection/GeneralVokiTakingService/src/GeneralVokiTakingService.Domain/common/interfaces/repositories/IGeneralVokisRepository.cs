using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IGeneralVokisRepository
{
    Task Add(GeneralVoki voki, CancellationToken ct = default);
    Task<GeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithResultsByIdAsNoTracking(VokiId vokiId, CancellationToken ct);
}