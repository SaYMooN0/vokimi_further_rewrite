using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Infrastructure.parsers;

public static class VokiAnswerTypeDataParser
{
    public static Dictionary<string, string> ToDictionary(BaseVokiAnswerTypeData data) => data.Match(
        textOnly: d => new Dictionary<string, string> { ["Text"] = d.Text },
        imageOnly: d => new() { ["Image"] = d.Image.ToString() },
        imageAndText: d => new() { ["Text"] = d.Text, ["Image"] = d.Image.ToString() },
        colorOnly: d => new() { ["Color"] = d.Color.ToString() },
        colorAndText: d => new() { ["Text"] = d.Text, ["Color"] = d.Color.ToString() },
        audioOnly: d => new() { ["Audio"] = d.Audio.ToString() },
        audioAndText: d => new() { ["Text"] = d.Text, ["Audio"] = d.Audio.ToString() }
    );


    public static ErrOr<BaseVokiAnswerTypeData> CreateFromDictionary(
        GeneralVokiAnswerType type,
        Dictionary<string, string> dictionary
    ) => type.Match(
        textOnly: () => CreateTextOnly(dictionary),
        imageOnly: () => CreateImageOnly(dictionary),
        imageAndText: () => CreateImageAndText(dictionary),
        colorOnly: () => CreateColorOnly(dictionary),
        colorAndText: () => CreateColorAndText(dictionary),
        audioOnly: () => CreateAudioOnly(dictionary),
        audioAndText: () => CreateAudioAndText(dictionary)
    );

    public static ErrOr<BaseVokiAnswerTypeData> CreateTextOnly(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Text", out string text)) {
            return ErrFactory.NoValue.Common("Unable to create text only answer data. Text field not provided");
        }

        var result = BaseVokiAnswerTypeData.TextOnly.CreateNew(text);
        if (result.IsErr(out var err)) {
            return err;
        }

        return result.AsSuccess();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageOnly(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageAndText(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateColorOnly(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateColorAndText(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioOnly(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioAndText(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
}