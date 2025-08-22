using System.Text.Json;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters;

public class VokiAnswerTypeDataConverter : ValueConverter<BaseVokiAnswerTypeData, string>
{
    public VokiAnswerTypeDataConverter() : base(
        data => ToString(data),
        str => FromString(str)
    ) { }

    private const string Divider = ": ";

    private static readonly JsonSerializerOptions JsonOpts = new() {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    private static string ToString(BaseVokiAnswerTypeData value) =>
        value.MatchingEnum + Divider + JsonSerializer.Serialize(ToDictionary(value), JsonOpts);

    private static BaseVokiAnswerTypeData FromString(string str) {
        string[] parts = str.Split(Divider, 2);
        var type = Enum.Parse<GeneralVokiAnswerType>(parts[0]);
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(parts[1], JsonOpts)!;
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
            .Bind<BaseVokiAnswerTypeData>(img => new BaseVokiAnswerTypeData.ImageOnly(new(img)));

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageAndText(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Text", "Unable to create image and text answer data. 'Text' field not provided")
            .Bind(GeneralVokiAnswerText.Create)
            .Bind<BaseVokiAnswerTypeData>(text =>
                Get(dictionary, "Image", "Unable to create image and text answer data. 'Image' field not provided")
                    .Bind<BaseVokiAnswerTypeData>(img => new BaseVokiAnswerTypeData.ImageAndText(text, new(img)))
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
            .Bind<BaseVokiAnswerTypeData>(a => new BaseVokiAnswerTypeData.AudioOnly(new(a)));

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioAndText(Dictionary<string, string> dictionary) =>
        Get(dictionary, "Text", "Unable to create audio and text answer data. 'Text' field not provided")
            .Bind(GeneralVokiAnswerText.Create)
            .Bind<BaseVokiAnswerTypeData>(text =>
                Get(dictionary, "Audio", "Unable to create audio and text answer data. 'Audio' field not provided")
                    .Bind<BaseVokiAnswerTypeData>(audio => new BaseVokiAnswerTypeData.AudioAndText(text, new(audio)))
            );
}