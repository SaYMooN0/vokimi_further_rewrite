using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answer_type_data;

public abstract partial class BaseVokiAnswerTypeData : ValueObject
{
    public abstract GeneralVokiAnswerType MatchingEnum { get; }
}