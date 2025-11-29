using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories;

public interface IGeneralVokisRepository 
{
    Task Add(GeneralVoki voki, CancellationToken ct);
    Task<GeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithResultsByIdAsNoTracking(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetById(VokiId vokiId, CancellationToken ct);
    Task Update(GeneralVoki voki, CancellationToken ct);
}