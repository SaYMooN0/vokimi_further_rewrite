using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.questions;

public record class VokiQuestionsOverviewResponse(
    VokiQuestionBriefDataResponse[] Questions,
    VokiTakingProcessSettings Settings
) : ICreatableResponse<GetVokiQuestionsOverviewQueryResult>
{

    public static ICreatableResponse<GetVokiQuestionsOverviewQueryResult> Create(GetVokiQuestionsOverviewQueryResult voki) =>
        new VokiQuestionsOverviewResponse(
        voki.Questions
            .Select(VokiQuestionBriefDataResponse.Create)
            .OrderBy(a => a.OrderInVoki)
            .ToArray(),
        voki.TakingProcessSettings
    );
}