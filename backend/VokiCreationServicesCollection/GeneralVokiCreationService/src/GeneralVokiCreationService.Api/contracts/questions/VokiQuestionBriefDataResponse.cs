using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

public record class VokiQuestionBriefDataResponse(
    string Id,
    string Text,
    string[] Images,
    GeneralVokiAnswerType AnswersType,
    ushort OrderInVoki,
    bool IsMultipleChoice
)
{
    public static VokiQuestionBriefDataResponse Create(VokiQuestion question) => new(
        question.Id.ToString(),
        question.Text.ToString(),
        question.Images.Keys.Select(k => k.ToString()).ToArray(),
        question.AnswersType,
        question.OrderInVoki,
        question.AnswersCountLimit.IsMultipleChoice
    );
}