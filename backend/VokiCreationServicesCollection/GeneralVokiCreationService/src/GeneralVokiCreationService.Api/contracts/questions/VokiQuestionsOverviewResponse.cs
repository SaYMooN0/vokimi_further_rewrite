using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.questions;

public record class VokiQuestionsOverviewResponse(
    VokiQuestionBriefDataResponse[] Questions,
    VokiTakingProcessSettings VokiTakingProcessSettings
)
{
    public static VokiQuestionsOverviewResponse Create(DraftGeneralVoki voki) => new(
        voki.Questions.Select(VokiQuestionBriefDataResponse.Create).ToArray(),
        voki.TakingProcessSettings
    );
}