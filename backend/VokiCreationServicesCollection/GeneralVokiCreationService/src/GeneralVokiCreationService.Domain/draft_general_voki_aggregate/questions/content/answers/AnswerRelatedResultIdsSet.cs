namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;

public sealed class AnswerRelatedResultIdsSet : ValueObject
{
    public const int MaxRelatedResultsForAnswerCount = 30;
    private ImmutableHashSet<GeneralVokiResultId> ResultIds { get; }

    public bool IsEmpty => ResultIds.Count == 0;

    private AnswerRelatedResultIdsSet(ImmutableHashSet<GeneralVokiResultId> resultIds) {
        ResultIds = resultIds;
    }

    public static ErrOr<AnswerRelatedResultIdsSet> Create(
        ImmutableHashSet<GeneralVokiResultId>? resultIds
    ) {
        if (resultIds is null) {
            return ErrFactory.NoValue.Common("Related result ids are required");
        }

        if (resultIds.Count > MaxRelatedResultsForAnswerCount) {
            return ErrFactory.LimitExceeded(
                "Too many related results for answer selected",
                $"Maximum allowed count: {MaxRelatedResultsForAnswerCount}. Selected : {resultIds.Count}"
            );
        }

        return new AnswerRelatedResultIdsSet(resultIds);
    }

    public int Count => ResultIds.Count;
    public override IEnumerable<object> GetEqualityComponents() => [..ResultIds];

    public AnswerRelatedResultIdsSet Remove(GeneralVokiResultId id) => new(ResultIds.Remove(id));

    public IEnumerable<T> Select<T>(Func<GeneralVokiResultId, T> func) => ResultIds.Select(func);
    public GeneralVokiResultId[] ToArray() => ResultIds.ToArray();
}