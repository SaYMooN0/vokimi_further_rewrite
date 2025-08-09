namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class TextOnly : BaseVokiAnswerTypeData
    {
        public GeneralVokiAnswerText Text { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.TextOnly;

        public TextOnly(GeneralVokiAnswerText text) {
            Text = text;
        }
        public override IEnumerable<object> GetEqualityComponents() => [Text];
    }
}