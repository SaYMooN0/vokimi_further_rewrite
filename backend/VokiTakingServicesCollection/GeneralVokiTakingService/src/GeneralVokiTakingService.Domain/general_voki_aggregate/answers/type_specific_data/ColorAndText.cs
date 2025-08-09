namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ColorAndText : BaseVokiAnswerTypeData
    {
        public GeneralVokiAnswerText Text { get; }
        public HexColor Color { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ColorAndText;

        public ColorAndText(GeneralVokiAnswerText text, HexColor color) {
            Text = text;
            Color = color;
        }
        public override IEnumerable<object> GetEqualityComponents() => [Text, Color];
    }
}