using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Application.dtos;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

internal record class VokiQuestionFullDataResponse(
    string Id,
    string Text,
    QuestionImageSetResponse ImageSet,
    GeneralVokiAnswerType AnswersType,
    IQuestionContentPrimitiveDto Content,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    Dictionary<string, string> ResultsIdToName
) : ICreatableResponse<GetVokiQuestionWithAnswersAndResultsQueryResult>
{
    public static ICreatableResponse<GetVokiQuestionWithAnswersAndResultsQueryResult> Create(
        GetVokiQuestionWithAnswersAndResultsQueryResult queryRes
    ) => new VokiQuestionFullDataResponse(
        queryRes.Question.Id.ToString(),
        queryRes.Question.Text.ToString(),
        QuestionImageSetResponse.Create(queryRes.Question.ImageSet),
        queryRes.Question.Content.AnswersType,
        IQuestionContentPrimitiveDto.FromQuestionContent(queryRes.Question.Content),
        queryRes.Question.ShuffleAnswers,
        queryRes.Question.AnswersCountLimit.MinAnswers,
        queryRes.Question.AnswersCountLimit.MaxAnswers,
        queryRes.ResultsIdToName.ToDictionary(
            r => r.Key.ToString(),
            r => r.Value.ToString()
        )
    );
}

internal record QuestionImageSetResponse(double Width, double Height, string[] Keys)
{
    public static QuestionImageSetResponse Create(VokiQuestionImagesSet set) => new(
        set.AspectRatio.Width,
        set.AspectRatio.Height,
        set.Keys.Select(imageKey => imageKey.ToString()).ToArray()
    );
}