using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.draft_general_voki.answer_audio;
using VokimiStorageKeysLib.draft_general_voki.answer_image;

namespace GeneralVokiCreationService.Api.contracts;

public record VokiAnswerTypeDataDto(
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

    private ErrOr<string> GetText() => Text is null
        ? ErrFactory.NoValue.Common("'Text' value is not provided")
        : Text;

    private ErrOr<DraftGeneralVokiAnswerImageKey> GetImage() => Image is null
        ? ErrFactory.NoValue.Common("'Image' value is not provided")
        : DraftGeneralVokiAnswerImageKey.Create(Image);

    private ErrOr<DraftGeneralVokiAnswerAudioKey> GetAudio() => Audio is null
        ? ErrFactory.NoValue.Common("'Audio' value is not provided")
        : DraftGeneralVokiAnswerAudioKey.Create(Audio);

    private ErrOr<HexColor> GetColor() => Color is null
        ? ErrFactory.NoValue.Common("'Color' value is not provided")
        : HexColor.Create(Color);

    public ErrOr<BaseVokiAnswerTypeData> ParseToAnswerData(GeneralVokiAnswerType type) => type.Match(
        textOnly: () => GetText()
            .Bind(BaseVokiAnswerTypeData.TextOnly.CreateNew)
            .Bind<BaseVokiAnswerTypeData>(d => d),
        imageOnly: () => GetImage().Bind<BaseVokiAnswerTypeData>(i =>
            new BaseVokiAnswerTypeData.ImageOnly(i)
        ),
        imageAndText: () => {
            var text = GetText();
            if (text.IsErr(out var err)) {
                return err;
            }

            var image = GetImage();
            if (image.IsErr(out err)) {
                return err;
            }

            return BaseVokiAnswerTypeData.ImageAndText.CreateNew(text.AsSuccess(), image.AsSuccess())
                .Bind<BaseVokiAnswerTypeData>(d => d);
        },
        colorOnly: () => GetColor().Bind<BaseVokiAnswerTypeData>(c =>
            new BaseVokiAnswerTypeData.ColorOnly(c)
        ),
        colorAndText: () => {
            var text = GetText();
            if (text.IsErr(out var err)) {
                return err;
            }

            var color = GetColor();
            if (color.IsErr(out err)) {
                return err;
            }

            return BaseVokiAnswerTypeData.ColorAndText.CreateNew(text.AsSuccess(), color.AsSuccess())
                .Bind<BaseVokiAnswerTypeData>(d => d);
            ;
        },
        audioOnly: () => GetAudio().Bind<BaseVokiAnswerTypeData>(a =>
            new BaseVokiAnswerTypeData.AudioOnly(a)
        ),
        audioAndText: () => {
            var text = GetText();
            if (text.IsErr(out var err)) {
                return err;
            }

            var audio = GetAudio();
            if (audio.IsErr(out err)) {
                return err;
            }

            return BaseVokiAnswerTypeData.AudioAndText.CreateNew(text.AsSuccess(), audio.AsSuccess())
                .Bind<BaseVokiAnswerTypeData>(d => d);
        }
    );

    public static VokiAnswerTypeDataDto FromAnswerData(BaseVokiAnswerTypeData data) =>
        data.Match<VokiAnswerTypeDataDto>(
            textOnly: d => new(Text: d.Text, Image: null, Audio: null, Color: null),
            imageOnly: d => new(Text: null, Image: d.Image.ToString(), Audio: null, Color: null),
            imageAndText: d => new(Text: d.Text, Image: d.Image.ToString(), Audio: null, Color: null),
            colorOnly: d => new(Text: null, Image: null, Audio: null, Color: d.Color.ToString()),
            colorAndText: d => new(Text: d.Text, Image: null, Audio: null, Color: d.Color.ToString()),
            audioOnly: d => new(Text: null, Image: null, Audio: d.Audio.ToString(), Color: null),
            audioAndText: d => new(Text: d.Text, Image: null, Audio: d.Audio.ToString(), Color: null)
        );
}