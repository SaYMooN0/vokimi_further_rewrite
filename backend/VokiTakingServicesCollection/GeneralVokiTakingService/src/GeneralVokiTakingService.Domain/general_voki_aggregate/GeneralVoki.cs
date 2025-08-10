using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using VokimiStorageKeysLib;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public sealed class GeneralVoki : AggregateRoot<VokiId>
{
    private GeneralVoki() { }
    private ImmutableArray<VokiQuestion> Questions { get; }
    public bool ForceSequentialAnswering { get; }
    private bool ShuffleQuestions { get; }
    private ImmutableArray<VokiResult> Results { get; }
    public ImmutableHashSet<VokiTakenRecordId> VokiTakenRecordIds { get; private set; }

    public ImmutableArray<VokiQuestion> OrderedQuestionForTaking() {
        if (ShuffleQuestions) {
            return Questions.OrderBy(_ => Guid.NewGuid()).ToImmutableArray();
        }

        return Questions.OrderBy(q => q.OrderInVoki).ToImmutableArray();
    }

    public GeneralVoki(
        VokiId id,
        ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering, bool shuffleQuestions
    ) {
        Id = id;
        Questions = questions;
        Results = results;
        ForceSequentialAnswering = forceSequentialAnswering;
        ShuffleQuestions = shuffleQuestions;
        VokiTakenRecordIds = [];
    }

    public static GeneralVoki CreateNew(
        VokiId id,
        ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering, bool shuffleQuestions
    ) {
        var voki = new GeneralVoki(id, questions, results, forceSequentialAnswering, shuffleQuestions);
        List<BaseStorageKey> vokiContentKeys = [];
        voki.AddDomainEvent(new PublishedVokiCreatedEvent(voki.Id, vokiContentKeys.ToArray()));
        return voki;
    }
}