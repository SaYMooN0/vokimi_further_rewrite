using GeneralVokiTakingService.Application.common.dtos;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.start;

public record GeneralVokiTakingResponseQuestionData(
    string Id,
    string Text,
    string[] ImageKeys,
    double ImagesAspectRatio,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
) : ICreatableResponse<VokiTakingQuestionData>
{
    public static GeneralVokiTakingResponseQuestionData FromQuestion(VokiTakingQuestionData question) => new(
        question.Id.ToString(),
        question.Text,
        question.ImagesSet.Keys.Select(k => k.ToString()).ToArray(),
        question.ImagesSet.AspectRatio,
        question.OrderInVokiTaking.Value,
        question.MinAnswersCount,
        question.MaxAnswersCount
    );

    public static ICreatableResponse<VokiTakingQuestionData> Create(VokiTakingQuestionData question) => FromQuestion(question);
    public  class GeneralVokiTakingResponseQuestionContentData 
}
