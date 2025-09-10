using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IGeneralVokisRepository
{
    Task Add(GeneralVoki voki);
    public Task<GeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId);
    public Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId);
}