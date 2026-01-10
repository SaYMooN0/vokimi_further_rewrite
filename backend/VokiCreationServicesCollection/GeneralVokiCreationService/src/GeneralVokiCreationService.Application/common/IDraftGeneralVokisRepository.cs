using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.common;

public interface IDraftGeneralVokisRepository : IDraftVokiRepository
{
    new Task<DraftGeneralVoki?> GetById(VokiId generalVokiId, CancellationToken ct);

    async Task<BaseDraftVoki?> IDraftVokiRepository.GetById(VokiId vokiId, CancellationToken ct) =>
        await GetById(vokiId, ct);

    Task Add(DraftGeneralVoki voki, CancellationToken ct );
    Task Update(DraftGeneralVoki generalVoki, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestionsForUpdate(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId, CancellationToken ct );

    Task<DraftGeneralVoki?> GetWithQuestionAnswersForUpdate(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestionAnswers(VokiId vokiId, CancellationToken ct );

    Task<DraftGeneralVoki?> GetWithResultsForUpdate(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithResults(VokiId vokiId, CancellationToken ct );

    Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResultsForUpdate(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResults(VokiId vokiId, CancellationToken ct );
    Task Delete(DraftGeneralVoki voki, CancellationToken ct );
}