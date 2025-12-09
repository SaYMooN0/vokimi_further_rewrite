using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.common;

public interface IDraftGeneralVokisRepository : IDraftVokiRepository
{
    new Task<DraftGeneralVoki?> GetById(VokiId generalVokiId, CancellationToken ct);
    new Task<DraftGeneralVoki?> GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct);

    async Task<BaseDraftVoki?> IDraftVokiRepository.GetByIdAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        await GetByIdAsNoTracking(vokiId, ct);

    Task Add(DraftGeneralVoki voki, CancellationToken ct );
    Task Update(DraftGeneralVoki generalVoki, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestionsAsNoTracking(VokiId vokiId, CancellationToken ct );

    Task<DraftGeneralVoki?> GetWithQuestionAnswers(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId, CancellationToken ct );

    Task<DraftGeneralVoki?> GetWithResults(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithResultsAsNoTracking(VokiId vokiId, CancellationToken ct );

    Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResults(VokiId vokiId, CancellationToken ct );
    Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId, CancellationToken ct );
    Task Delete(DraftGeneralVoki voki, CancellationToken ct );
}