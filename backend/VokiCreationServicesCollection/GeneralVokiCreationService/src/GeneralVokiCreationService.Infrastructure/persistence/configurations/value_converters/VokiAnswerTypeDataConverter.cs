using System.Text.Json;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.draft_general_voki.answer_audio;
using VokimiStorageKeysLib.draft_general_voki.answer_image;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters;

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
        textOnly: d => new Dictionary<string, string> { ["Text"] = d.Text },
        imageOnly: d => new() { ["Image"] = d.Image.ToString() },
        imageAndText: d => new() { ["Text"] = d.Text, ["Image"] = d.Image.ToString() },
        colorOnly: d => new() { ["Color"] = d.Color.ToString() },
        colorAndText: d => new() { ["Text"] = d.Text, ["Color"] = d.Color.ToString() },
        audioOnly: d => new() { ["Audio"] = d.Audio.ToString() },
        audioAndText: d => new() { ["Text"] = d.Text, ["Audio"] = d.Audio.ToString() }
    );


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

    private static ErrOr<BaseVokiAnswerTypeData> CreateTextOnly(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Text", out string text)) {
            return ErrFactory.NoValue.Common("Unable to create text only answer data. 'Text' field not provided");
        }

        var result = BaseVokiAnswerTypeData.TextOnly.CreateNew(text);
        if (result.IsErr(out var err)) {
            return err;
        }

        return result.AsSuccess();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageOnly(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Image", out string image)) {
            return ErrFactory.NoValue.Common("Unable to create image only answer data. 'Image' field not provided");
        }

        var res = DraftGeneralVokiAnswerImageKey.Create(image);
        if (res.IsErr(out var err)) {
            return err;
        }

        var key = res.AsSuccess();
        return new BaseVokiAnswerTypeData.ImageOnly(key);
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateImageAndText(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Text", out string text)) {
            return ErrFactory.NoValue.Common("Unable to create image and text answer data. 'Text' field not provided");
        }

        if (!dictionary.TryGetValue("Image", out string image)) {
            return ErrFactory.NoValue.Common("Unable to create image and text answer data. 'Image' field not provided");
        }

        var res = DraftGeneralVokiAnswerImageKey.Create(image);
        if (res.IsErr(out var err)) {
            return err;
        }

        var key = res.AsSuccess();
        var creationRes = BaseVokiAnswerTypeData.ImageAndText.CreateNew(text, key);
        if (creationRes.IsErr(out err)) {
            return err;
        }

        return creationRes.AsSuccess();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateColorOnly(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Color", out string color)) {
            return ErrFactory.NoValue.Common("Unable to create color only answer data. 'Color' field not provided");
        }

        var res = HexColor.Create(color);
        if (res.IsErr(out var err)) {
            return err;
        }

        var hexColor = res.AsSuccess();
        return new BaseVokiAnswerTypeData.ColorOnly(hexColor);
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateColorAndText(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Text", out string text)) {
            return ErrFactory.NoValue.Common("Unable to create color and text answer data. 'Text' field not provided");
        }

        if (!dictionary.TryGetValue("Color", out string color)) {
            return ErrFactory.NoValue.Common("Unable to create color and text answer data. 'Color' field not provided");
        }

        var res = HexColor.Create(color);
        if (res.IsErr(out var err)) {
            return err;
        }

        var hexColor = res.AsSuccess();
        var creationRes = BaseVokiAnswerTypeData.ColorAndText.CreateNew(text, hexColor);
        if (creationRes.IsErr(out err)) {
            return err;
        }

        return creationRes.AsSuccess();
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioOnly(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Audio", out string audio)) {
            return ErrFactory.NoValue.Common("Unable to create audio only answer data. 'Audio' field not provided");
        }

        var res = DraftGeneralVokiAnswerAudioKey.Create(audio);
        if (res.IsErr(out var err)) {
            return err;
        }

        var key = res.AsSuccess();
        return new BaseVokiAnswerTypeData.AudioOnly(key);
    }

    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioAndText(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Text", out string text)) {
            return ErrFactory.NoValue.Common("Unable to create audio and text answer data. 'Text' field not provided");
        }

        if (!dictionary.TryGetValue("Audio", out string audio)) {
            return ErrFactory.NoValue.Common("Unable to create audio and text answer data. 'Audio' field not provided");
        }

        var res = DraftGeneralVokiAnswerAudioKey.Create(audio);
        if (res.IsErr(out var err)) {
            return err;
        }

        var key = res.AsSuccess();
        var creationRes = BaseVokiAnswerTypeData.AudioAndText.CreateNew(text, key);
        if (creationRes.IsErr(out err)) {
            return err;
        }

        return creationRes.AsSuccess();
    }
}