namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ColorOnly : BaseVokiAnswerTypeData
    {
        public HexColor Color { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ColorOnly;

        public ColorOnly(HexColor color) {
            Color = color;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Color];

    }
}