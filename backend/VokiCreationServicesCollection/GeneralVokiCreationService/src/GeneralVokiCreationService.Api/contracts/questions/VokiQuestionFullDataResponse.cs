using GeneralVokiCreationService.Api.contracts.results;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

internal record class VokiQuestionFullDataResponse(
    string Id,
    string Text,
    QuestionImageSetResponse ImageSet,
    GeneralVokiAnswerType AnswersType,
    VokiQuestionAnswerResponse[] Answers,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    VokiResultIdWithNameResponse[] Results
)
{
    public static VokiQuestionFullDataResponse Create(VokiQuestion question, ImmutableArray<VokiResult> results) => new(
        question.Id.ToString(),
        question.Text.ToString(),
        QuestionImageSetResponse.Create(question.ImageSet),
        question.AnswersType,
        question.Answers.Select(VokiQuestionAnswerResponse.Create).OrderBy(a => a.Order).ToArray(),
        question.ShuffleAnswers,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers,
        results.Select(VokiResultIdWithNameResponse.Create).ToArray()
    );
}

internal record  QuestionImageSetResponse(double Width, double Height, string[] Keys)
{
    public static QuestionImageSetResponse Create(VokiQuestionImagesSet set) => new(
        set.AspectRatio.Width,
        set.AspectRatio.Height,
        set.Keys.Select(imageKey => imageKey.ToString()).ToArray()
    );
}