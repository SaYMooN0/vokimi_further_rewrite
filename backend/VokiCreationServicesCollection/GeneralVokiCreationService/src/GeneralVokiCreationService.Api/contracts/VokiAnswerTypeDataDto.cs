using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Api.contracts;

public record VokiAnswerTypeDataDto(
    GeneralVokiAnswerType Type,
    string? Text,
    string? Image,
    string? Audio,
    string? Color
)
{
    public bool IsEmpty() =>
        string.IsNullOrEmpty(Text)
        && string.IsNullOrEmpty(Image)
        && string.IsNullOrEmpty(Audio)
        && string.IsNullOrEmpty(Color);

    private ErrOr<GeneralVokiAnswerText> GetText() => Text is null
        ? ErrFactory.NoValue.Common("'Text' value is not provided")
        : GeneralVokiAnswerText.Create(Text);

    private ErrOr<GeneralVokiAnswerImageKey> GetImage() => Image is null
        ? ErrFactory.NoValue.Common("'Image' value is not provided")
        : GeneralVokiAnswerImageKey.FromString(Image);

    private ErrOr<GeneralVokiAnswerAudioKey> GetAudio() => Audio is null
        ? ErrFactory.NoValue.Common("'Audio' value is not provided")
        : GeneralVokiAnswerAudioKey.FromString(Audio);

    private ErrOr<HexColor> GetColor() => Color is null
        ? ErrFactory.NoValue.Common("'Color' value is not provided")
        : HexColor.Create(Color);

    public ErrOr<BaseVokiAnswerTypeData> ParseToAnswerData() => Type.Match(
        textOnly: () => GetText().Bind<BaseVokiAnswerTypeData>(
            text => new BaseVokiAnswerTypeData.TextOnly(text)
        ),
        imageOnly: () => GetImage().Bind<BaseVokiAnswerTypeData>(
            image => new BaseVokiAnswerTypeData.ImageOnly(image)
        ),
        imageAndText: () =>
            GetText().Bind<BaseVokiAnswerTypeData>(
                text => GetImage().Bind<BaseVokiAnswerTypeData>(
                    image => new BaseVokiAnswerTypeData.ImageAndText(text, image)
                )
            ),
        colorOnly: () => GetColor().Bind<BaseVokiAnswerTypeData>(
            color => new BaseVokiAnswerTypeData.ColorOnly(color)
        ),
        colorAndText: () =>
            GetText().Bind<BaseVokiAnswerTypeData>(
                text => GetColor().Bind<BaseVokiAnswerTypeData>(
                    color => new BaseVokiAnswerTypeData.ColorAndText(text, color)
                )
            ),
        audioOnly: () => GetAudio().Bind<BaseVokiAnswerTypeData>(
            audio => new BaseVokiAnswerTypeData.AudioOnly(audio)
        ),
        audioAndText: () =>
            GetText().Bind<BaseVokiAnswerTypeData>(
                text => GetAudio().Bind<BaseVokiAnswerTypeData>(
                    audio => new BaseVokiAnswerTypeData.AudioAndText(text, audio)
                )
            )
    );

    public static VokiAnswerTypeDataDto FromAnswerData(BaseVokiAnswerTypeData data) =>
        data.Match<VokiAnswerTypeDataDto>(
            textOnly: d => new(
                GeneralVokiAnswerType.TextOnly,
                Text: d.Text.ToString(), Image: null, Audio: null, Color: null
            ),
            imageOnly: d => new(
                GeneralVokiAnswerType.ImageOnly,
                Text: null, Image: d.Image.ToString(), Audio: null, Color: null
            ),
            imageAndText: d => new(
                GeneralVokiAnswerType.ImageAndText,
                Text: d.Text.ToString(), Image: d.Image.ToString(), Audio: null, Color: null
            ),
            colorOnly: d => new(
                GeneralVokiAnswerType.ColorOnly,
                Text: null, Image: null, Audio: null, Color: d.Color.ToString()
            ),
            colorAndText: d => new(
                GeneralVokiAnswerType.ColorAndText,
                Text: d.Text.ToString(), Image: null, Audio: null, Color: d.Color.ToString()
            ),
            audioOnly: d => new(
                GeneralVokiAnswerType.AudioOnly,
                Text: null, Image: null, Audio: d.Audio.ToString(), Color: null
            ),
            audioAndText: d => new(
                GeneralVokiAnswerType.AudioAndText,
                Text: d.Text.ToString(), Image: null, Audio: d.Audio.ToString(), Color: null
            )
        );
}