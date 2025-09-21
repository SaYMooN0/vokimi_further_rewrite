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
        bool authenticatedOnlyTaking
    ) : base(
        id, name, cover, primaryAuthorId, coAuthorIds, details, tags, publishedDate, authenticatedOnlyTaking
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
        bool authenticatedOnlyTaking
    ) {
        var voki = new GeneralVoki(
            id, name, cover, primaryAuthorId, coAuthorIds,
            details, tags, publishedDate,
            questionsCount, resultsCount, anyAudioAnswers,
            authenticatedOnlyTaking
        );
        voki.AddDomainEvent(
            new PublishedVokiCreatedEvent(voki.Id, voki.PrimaryAuthorId, voki.CoAuthorIds, voki.Tags)
        );

        return voki;
    }
}