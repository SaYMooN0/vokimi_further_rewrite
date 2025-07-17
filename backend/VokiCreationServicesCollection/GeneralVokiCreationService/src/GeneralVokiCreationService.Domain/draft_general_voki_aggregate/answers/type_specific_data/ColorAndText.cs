using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ColorAndText : BaseVokiAnswerTypeData
    {
        public string Text { get; }
        public HexColor Color { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ColorAndText;

        private ColorAndText(string text, HexColor color) {
            InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiAnswerRules.CheckAnswerTextForErrs(text));
            Text = text;
            Color = color;
        }

        public static ErrOr<ColorAndText> CreateNew(string text, HexColor color) =>
            GeneralVokiAnswerRules.CheckAnswerTextForErrs(text).IsErr(out var err)
                ? err
                : new ColorAndText(text, color);

        public override IEnumerable<object> GetEqualityComponents() => [Text, Color];
    }
}