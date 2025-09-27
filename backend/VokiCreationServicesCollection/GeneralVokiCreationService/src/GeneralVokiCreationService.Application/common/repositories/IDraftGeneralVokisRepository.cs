using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.repositories;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.common.repositories;

public interface IDraftGeneralVokisRepository : IDraftVokiRepository
{
    Task<DraftGeneralVoki?> GetById(VokiId vokiId);
    new Task<DraftGeneralVoki?> GetByIdAsNoTracking(VokiId vokiId);

    async Task<BaseDraftVoki?> IDraftVokiRepository.GetByIdAsNoTracking(VokiId vokiId) =>
        await GetByIdAsNoTracking(vokiId);

    Task Add(DraftGeneralVoki voki);
    Task Update(DraftGeneralVoki voki);

    Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId);
    Task<DraftGeneralVoki?> GetWithQuestionsAsNoTracking(VokiId vokiId);

    Task<DraftGeneralVoki?> GetWithQuestionAnswers(VokiId vokiId);
    Task<DraftGeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId);

    Task<DraftGeneralVoki?> GetWithResults(VokiId vokiId);
    Task<DraftGeneralVoki?> GetWithResultsAsNoTracking(VokiId vokiId);

    Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResults(VokiId vokiId);
    Task<DraftGeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId);
    Task Delete(DraftGeneralVoki voki);
}