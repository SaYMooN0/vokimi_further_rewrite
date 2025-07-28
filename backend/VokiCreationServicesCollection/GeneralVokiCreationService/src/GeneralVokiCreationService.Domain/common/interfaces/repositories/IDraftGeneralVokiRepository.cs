using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.repositories;

namespace GeneralVokiCreationService.Domain.common.interfaces.repositories;

public interface IDraftGeneralVokiRepository : IDraftVokiRepository
{
    Task Add(DraftGeneralVoki voki);

    new Task<DraftGeneralVoki?> GetByIdAsNoTracking(VokiId vokiId);
     Task<DraftGeneralVoki?> GetWithQuestionsAsNoTracking(VokiId vokiId);


    Task<DraftGeneralVoki?> GetById(VokiId vokiId);
    Task Update(DraftGeneralVoki voki);
    async Task<BaseDraftVoki?> IDraftVokiRepository.GetByIdAsNoTracking(VokiId vokiId) =>
        await GetByIdAsNoTracking(vokiId);

    Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId);
    Task<DraftGeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId);
    Task<DraftGeneralVoki?> GetWithQuestionAnswers(VokiId vokiId);
}
