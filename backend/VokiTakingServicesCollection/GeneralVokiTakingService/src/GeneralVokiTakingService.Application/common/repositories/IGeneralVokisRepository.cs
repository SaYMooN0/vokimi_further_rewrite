using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories;

public interface IGeneralVokisRepository 
{
    Task Add(GeneralVoki voki, CancellationToken ct);
    Task<GeneralVoki?> GetWithQuestionAnswersAndResults(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithQuestionAnswers(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetWithResultsById(VokiId vokiId, CancellationToken ct);
    Task<GeneralVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct);
    Task Update(GeneralVoki voki, CancellationToken ct);
}