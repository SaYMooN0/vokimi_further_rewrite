using System.Text.Json;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.general_voki.answer_audio;
using VokimiStorageKeysLib.general_voki.answer_image;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;

public class VokiAnswerTypeDataConverter : ValueConverter<BaseVokiAnswerTypeData, string>
{
    public VokiAnswerTypeDataConverter() : base(
        data => ToString(data),
        str => FromString(str)
    ) { }

    private const string Divider = ": ";

    private static string ToString(BaseVokiAnswerTypeData value) =>
        value.MatchingEnum + Divider + JsonSerializer.Serialize(ToDictionary(value));

    private static BaseVokiAnswerTypeData FromString(string str) {
        string[] parts = str.Split(Divider, 2);
        GeneralVokiAnswerType type = Enum.Parse<GeneralVokiAnswerType>(parts[0]);
        Dictionary<string, string> data = JsonSerializer.Deserialize<Dictionary<string, string>>(parts[1])!;
        return CreateFromDictionary(type, data).AsSuccess();
    }

    private static Dictionary<string, string> ToDictionary(BaseVokiAnswerTypeData data) => data.Match(
        textOnly: d => new Dictionary<string, string> { ["Text"] = d.Text.ToString() },
        imageOnly: d => new() { ["Image"] = d.Image.ToString() },
        imageAndText: d => new() { ["Text"] = d.Text.ToString(), ["Image"] = d.Image.ToString() },
        colorOnly: d => new() { ["Color"] = d.Color.ToString() },
        colorAndText: d => new() { ["Text"] = d.Text.ToString(), ["Color"] = d.Color.ToString() },
        audioOnly: d => new() { ["Audio"] = d.Audio.ToString() },
        audioAndText: d => new() { ["Text"] = d.Text.ToString(), ["Audio"] = d.Audio.ToString() }
    );

    private static ErrOr<string> Get(Dictionary<string, string> dict, string key, string err) =>
        dict.TryGetValue(key, out var v) ? v : ErrFactory.NoValue.Common(err);

    private static ErrOr<BaseVokiAnswerTypeData> CreateFromDictionary(
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

    private static ErrOr<BaseVokiAnswerTypeData> CreateTextOnly(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Text", "Unable to create text only answer data. 'Text' field not provided")
            .Bind(GeneralVokiAnswerText.Create)
            .Bind<BaseVokiAnswerTypeData>(t => new BaseVokiAnswerTypeData.TextOnly(t));

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageOnly(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Image", "Unable to create image only answer data. 'Image' field not provided")
            .Bind(GeneralVokiAnswerImageKey.Create)
            .Bind<BaseVokiAnswerTypeData>(img => new BaseVokiAnswerTypeData.ImageOnly(img));

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageAndText(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Text", "Unable to create image and text answer data. 'Text' field not provided")
            .Bind(GeneralVokiAnswerText.Create)
            .Bind<BaseVokiAnswerTypeData>(text =>
                Get(dictionary, "Image", "Unable to create image and text answer data. 'Image' field not provided")
                    .Bind(GeneralVokiAnswerImageKey.Create)
                    .Bind<BaseVokiAnswerTypeData>(img => new BaseVokiAnswerTypeData.ImageAndText(text, img))
            );

    private static ErrOr<BaseVokiAnswerTypeData> CreateColorOnly(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Color", "Unable to create color only answer data. 'Color' field not provided")
            .Bind(HexColor.Create)
            .Bind<BaseVokiAnswerTypeData>(c => new BaseVokiAnswerTypeData.ColorOnly(c));

    private static ErrOr<BaseVokiAnswerTypeData> CreateColorAndText(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Text", "Unable to create color and text answer data. 'Text' field not provided")
            .Bind(GeneralVokiAnswerText.Create)
            .Bind<BaseVokiAnswerTypeData>(text =>
                Get(dictionary, "Color", "Unable to create color and text answer data. 'Color' field not provided")
                    .Bind(HexColor.Create)
                    .Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorAndText(text, color))
            );

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioOnly(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Audio", "Unable to create audio only answer data. 'Audio' field not provided")
            .Bind(GeneralVokiAnswerAudioKey.Create)
            .Bind<BaseVokiAnswerTypeData>(a => new BaseVokiAnswerTypeData.AudioOnly(a));

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioAndText(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Text", "Unable to create audio and text answer data. 'Text' field not provided")
            .Bind(GeneralVokiAnswerText.Create)
            .Bind<BaseVokiAnswerTypeData>(text =>
                Get(dictionary, "Audio", "Unable to create audio and text answer data. 'Audio' field not provided")
                    .Bind(GeneralVokiAnswerAudioKey.Create)
                    .Bind<BaseVokiAnswerTypeData>(audio => new BaseVokiAnswerTypeData.AudioAndText(text, audio))
            );
}