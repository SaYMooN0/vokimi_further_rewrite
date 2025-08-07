namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers;

public abstract class BaseVokiQuestionAnswer : ValueObject
{
    public ushort OrderInQuestion { get; }
    public ImmutableHashSet<GeneralVokiResultId> RelatedResultIds { get; }
    public abstract GeneralVokiAnswerType MatchingEnum { get; }
}