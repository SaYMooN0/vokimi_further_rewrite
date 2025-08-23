namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public sealed record ColorOnly(
        HexColor Color
    ) : BaseVokiAnswerTypeData
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ColorOnly;
    }
}