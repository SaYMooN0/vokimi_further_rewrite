using VokisCatalogService.Domain.voki_aggregate.events;

namespace VokisCatalogService.Domain.voki_aggregate.voki_types;

public sealed class GeneralVoki : BaseVoki
{
    private GeneralVoki() { }
    public override VokiType VokiType => VokiType.General;
    public ushort QuestionsCount { get; }
    public ushort ResultsCount { get; }
    public bool AnyAudioAnswers { get; }

    private GeneralVoki(
        VokiId id, VokiName name,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds,
        ImmutableHashSet<VokiTagId> tags,
        ushort questionsCount, ushort resultsCount, bool anyAudioAnswers
    ) : base(
        id, name, primaryAuthorId, coAuthorIds, tags
    ) {
        QuestionsCount = questionsCount;
        ResultsCount = resultsCount;
        AnyAudioAnswers = anyAudioAnswers;
    }

    public static GeneralVoki CreateNew(
        VokiId id, VokiName name,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds,
        ImmutableHashSet<VokiTagId> tags,
        ushort questionsCount, ushort resultsCount, bool anyAudioAnswers
    ) {
        GeneralVoki voki = new(
            id, name, primaryAuthorId, coAuthorIds, tags,
            questionsCount, resultsCount, anyAudioAnswers
        );
        voki.AddDomainEvent(new PublishedVokiCreatedEvent(voki.Id));
        return voki;
    }
}