using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Domain.voki_aggregate.events;

namespace VokisCatalogService.Domain.voki_aggregate.voki_types;

public sealed class GeneralVoki : BaseVoki
{
    private GeneralVoki() { }
    public override VokiType Type => VokiType.General;
    public ushort QuestionsCount { get; }
    public ushort ResultsCount { get; }
    public bool AnyAudioAnswers { get; }

    private GeneralVoki(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        ushort questionsCount, ushort resultsCount, bool anyAudioAnswers,
        bool signedInOnlyTaking
    ) : base(
        id, name, cover, primaryAuthorId, coAuthorIds, details, tags, publishedDate, signedInOnlyTaking
    ) {
        QuestionsCount = questionsCount;
        ResultsCount = resultsCount;
        AnyAudioAnswers = anyAudioAnswers;
    }

    public static GeneralVoki CreateNew(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        ushort questionsCount, ushort resultsCount, bool anyAudioAnswers,
        bool signedInOnlyTaking
    ) {
        GeneralVoki voki = new GeneralVoki(
            id, name, cover, primaryAuthorId, coAuthorIds,
            details, tags, publishedDate,
            questionsCount, resultsCount, anyAudioAnswers,
            signedInOnlyTaking
        );
        voki.AddDomainEvent(
            new PublishedVokiCreatedEvent(voki.Id, voki.PrimaryAuthorId, voki.CoAuthorIds, voki.Tags)
        );

        return voki;
    }
}