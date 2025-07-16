using SharedKernel.common.vokis;
using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answer_type_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class TextOnly : BaseVokiAnswerTypeData
    {
        public string Text { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.TextOnly;

        private TextOnly(string text) {
            InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiAnswerRules.CheckAnswerTextForErrs(text));
            Text = text;
        }


        public static ErrOr<TextOnly> CreateNew(string text) =>
            GeneralVokiAnswerRules.CheckAnswerTextForErrs(text).IsErr(out var err)
                ? err
                : new TextOnly(text);

        public override IEnumerable<object> GetEqualityComponents() => [Text];
    }
}