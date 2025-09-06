using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Application.dtos;

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

    public ErrOr<GeneralVokiAnswerText> GetText() => Text is null
        ? ErrFactory.NoValue.Common("'Text' value is not provided")
        : GeneralVokiAnswerText.Create(Text);

   

    public ErrOr<HexColor> GetColor() => Color is null
        ? ErrFactory.NoValue.Common("'Color' value is not provided")
        : HexColor.Create(Color);

    
    public static VokiAnswerTypeDataDto FromAnswerData(BaseVokiAnswerTypeData data) => data.Match<VokiAnswerTypeDataDto>(
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