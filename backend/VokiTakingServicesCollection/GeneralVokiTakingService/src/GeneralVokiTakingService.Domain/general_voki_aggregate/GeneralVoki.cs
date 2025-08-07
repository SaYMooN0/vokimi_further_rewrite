namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public sealed class GeneralVoki : AggregateRoot<VokiId>
{
    private GeneralVoki() { }
    private ImmutableArray<VokiQuestion> Questions { get; }
    private ImmutableArray<VokiResult> Results { get; }
    public bool ForceSequentialAnswering { get; }
    private bool ShuffleQuestions { get; }

    public ImmutableArray<VokiQuestion> OrderedQuestionForTaking() {
        if (ShuffleQuestions) {
            return Questions.OrderBy(_ => Guid.NewGuid()).ToImmutableArray();
        }

        return Questions.OrderBy(q => q.OrderInVoki).ToImmutableArray();
    }

    public GeneralVoki(
        VokiId id,
        ImmutableArray<VokiQuestion> questions,
        ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering,
        bool shuffleQuestions
    ) {
        Id = id;
        Questions = questions;
        Results = results;
        ForceSequentialAnswering = forceSequentialAnswering;
        ShuffleQuestions = shuffleQuestions;
    }
}