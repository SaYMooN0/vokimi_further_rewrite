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
        BaseVokiAnswerTypeData typeData,
        ushort orderInQuestion,
        ImmutableHashSet<GeneralVokiResultId> relatedResultIds
    ) {
        if (relatedResultIds.Count > MaxRelatedResultsForAnswerCount) {
            return ErrFactory.LimitExceeded(
                "Too many related results for answer selected",
                $"Maximum allowes count: {MaxRelatedResultsForAnswerCount}. Selected : {relatedResultIds.Count}"
            );
        }

        return new VokiQuestionAnswer() {
            Id = GeneralVokiAnswerId.CreateNew(),
            OrderInQuestion = orderInQuestion,
            TypeData = typeData,
            RelatedResultIds = relatedResultIds
        };
    }
}