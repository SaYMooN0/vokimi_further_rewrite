using GeneralVokiTakingService.Application.general_vokis.commands;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;

namespace GeneralVokiTakingService.Api.contracts;

public record GeneralVokiTakingResponseQuestionData(
    string Id,
    string Text,
    string[] ImageKeys,
    double ImagesAspectRatio,
    GeneralVokiAnswerType AnswerType,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    GeneralVokiTakingResponseAnswerData[] Answers
)
{
    public static GeneralVokiTakingResponseQuestionData Create(StartVokiTakingCommandResponseQuestionData question) => new(
        question.Id.ToString(),
        question.Text,
        question.ImagesSet.Keys.Select(k => k.ToString()).ToArray(),
        question.ImagesSet.AspectRatio,
        question.AnswerType,
        question.OrderInVokiTaking,
        question.MinAnswersCount,
        question.MaxAnswersCount,
        question.Answers.Select(GeneralVokiTakingResponseAnswerData.Create).ToArray()
    );
}

public record GeneralVokiTakingResponseAnswerData(
    string Id,
    Dictionary<string, string> TypeData
)
{
    public static GeneralVokiTakingResponseAnswerData Create(VokiQuestionAnswer answer) => new(
        answer.Id.ToString(),
        AnswerTypeDataToDictionary(answer.TypeData)
    );

    private static Dictionary<string, string> AnswerTypeDataToDictionary(BaseVokiAnswerTypeData data) => data.Match(
        textOnly: d => new Dictionary<string, string> { ["Text"] = d.Text.ToString() },
        imageOnly: d => new() { ["Image"] = d.Image.ToString() },
        imageAndText: d => new() { ["Text"] = d.Text.ToString(), ["Image"] = d.Image.ToString() },
        colorOnly: d => new() { ["Color"] = d.Color.ToString() },
        colorAndText: d => new() { ["Text"] = d.Text.ToString(), ["Color"] = d.Color.ToString() },
        audioOnly: d => new() { ["Audio"] = d.Audio.ToString() },
        audioAndText: d => new() { ["Text"] = d.Text.ToString(), ["Audio"] = d.Audio.ToString() }
    );
}