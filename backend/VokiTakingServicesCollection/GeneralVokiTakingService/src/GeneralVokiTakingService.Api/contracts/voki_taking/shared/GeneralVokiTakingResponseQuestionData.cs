using System.Text.Json.Serialization;
using GeneralVokiTakingService.Application.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared;

public record GeneralVokiTakingResponseQuestionData(
    string Id,
    string Text,
    string[] ImageKeys,
    double ImagesAspectRatio,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    VokiTakingQuestionContentDto Content
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

    [JsonDerivedType(typeof(TextOnlyContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.TextOnly))]
    public abstract record GeneralVokiTakingResponseQuestionContentData()
    
    {
        public static GeneralVokiTakingResponseQuestionContentData Create(GeneralVokiQuestionContent content) => new();
    
        public sealed record TextOnlyContent(
        ) : GeneralVokiTakingResponseQuestionContentData
        {
            public sealed record
        }
    }
    
    public abstract record BaseContentAnswerData(string Id, ushort OrderInQuestion)
    {
        public sealed record TextOnly(string Id, ushort OrderInQuestion, string Text) : BaseContentAnswerData(Id, OrderInQuestion)
        {
            public static TextOnly Create(GeneralVokiAnswerText text,GeneralVokiAnswerId id,ushort orderInQuestion) => new(
                Id: id.ToString(),
                OrderInQuestion: orderInQuestion,
                Text: text.ToString()
            );
        }
    }
}