using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.parsers;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

internal record class VokiQuestionFullDataResponse(
    string Id,
    string Text,
    string[] Images,
    GeneralVokiAnswerType AnswersType,
    VokiQuestionAnswerResponse[] Answers,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
)
{
    public static VokiQuestionFullDataResponse Create(VokiQuestion question) => new(
        question.Id.ToString(),
        question.Text.ToString(),
        question.Images.Keys.Select(imageKey => imageKey.ToString()).ToArray(),
        question.AnswersType,
        question.Answers.Select(VokiQuestionAnswerResponse.Create).OrderBy(a => a.Order).ToArray(),
        question.ShuffleAnswers,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers
    );
}
