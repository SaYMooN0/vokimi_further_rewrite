using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public sealed record TextOnly(
        GeneralVokiAnswerText Text
    ) : BaseVokiAnswerTypeData
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.TextOnly;
    }
}