using SharedKernel.exceptions;
using VokimiStorageKeysLib.draft_general_voki.answer_image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ImageAndText : BaseVokiAnswerTypeData
    {
        public string Text { get; }
        public DraftGeneralVokiAnswerImageKey Image { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageAndText;

        private ImageAndText(string text, DraftGeneralVokiAnswerImageKey image) {
            InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiAnswerRules.CheckAnswerTextForErrs(text));
            Text = text;
            Image = image;
        }

        public static ErrOr<ImageAndText> CreateNew(string text, DraftGeneralVokiAnswerImageKey image) =>
            GeneralVokiAnswerRules.CheckAnswerTextForErrs(text).IsErr(out var err)
                ? err
                : new ImageAndText(text, image);

        public override IEnumerable<object> GetEqualityComponents() => [Text, Image];
    }
}