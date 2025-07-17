namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData : ValueObject
{
    public abstract GeneralVokiAnswerType MatchingEnum { get; }
}