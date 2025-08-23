using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestionAnswer : Entity<GeneralVokiAnswerId>
{
    private VokiQuestionAnswer() { }
    public const int MaxRelatedResultsForAnswerCount = 30;
    public ushort OrderInQuestion { get; private set; }
    public BaseVokiAnswerTypeData TypeData { get; private set; }
    public ImmutableHashSet<GeneralVokiResultId> RelatedResultIds { get; private set; }

    public static ErrOr<VokiQuestionAnswer> CreateNew(
        GeneralVokiAnswerId answerId,
        BaseVokiAnswerTypeData typeData,
        ushort orderInQuestion,
        ImmutableHashSet<GeneralVokiResultId> relatedResultIds
    ) {
        if (CheckRelatedResultsForErr(relatedResultIds).IsErr(out var err)) {
            return err;
        }

        return new VokiQuestionAnswer() {
            Id = answerId,
            OrderInQuestion = orderInQuestion,
            TypeData = typeData,
            RelatedResultIds = relatedResultIds
        };
    }

    private static ErrOrNothing CheckRelatedResultsForErr(ImmutableHashSet<GeneralVokiResultId> relatedResultIds) =>
        relatedResultIds.Count > MaxRelatedResultsForAnswerCount
            ? ErrFactory.LimitExceeded(
                "Too many related results for answer selected",
                $"Maximum allowed count: {MaxRelatedResultsForAnswerCount}. Selected : {relatedResultIds.Count}"
            )
            : ErrOrNothing.Nothing;


    public ErrOrNothing Update(
        BaseVokiAnswerTypeData newAnswerData,
        ImmutableHashSet<GeneralVokiResultId> newRelatedResultIds
    ) {
        if (TypeData.MatchingEnum != newAnswerData.MatchingEnum) {
            return ErrFactory.Conflict("Cannot update answer because new data type does not correspond with current");
        }

        if (CheckRelatedResultsForErr(newRelatedResultIds).IsErr(out var err)) {
            return err;
        }

        TypeData = newAnswerData;
        RelatedResultIds = newRelatedResultIds;
        return ErrOrNothing.Nothing;
    }

    public void RemoveRelatedResult(GeneralVokiResultId resultId) {
        RelatedResultIds = RelatedResultIds.Remove(resultId);
    }
}