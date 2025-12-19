using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Domain.voki_aggregate.events;

namespace VokisCatalogService.Domain.voki_aggregate.voki_types;

public sealed class GeneralVoki : BaseVoki
{
    private GeneralVoki() { }
    public override VokiType Type => VokiType.General;
    public override IVokiInteractionSettings BaseInteractionSettings => InteractionSettings;
    public GeneralVokiInteractionSettings InteractionSettings { get; }
    public ushort QuestionsCount { get; }
    public ushort ResultsCount { get; }
    public bool AnyAudios { get; }
    public bool ForceSequentialAnswering { get; }
    public bool ShuffleQuestions { get; }

    private GeneralVoki(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        GeneralVokiInteractionSettings interactionSettings,
        ushort questionsCount, ushort resultsCount, bool anyAudios,
        bool forceSequentialAnswering, bool shuffleQuestions
    ) : base(id, name, cover, primaryAuthorId, coAuthorIds, managers, details, tags, publishedDate) {
        ForceSequentialAnswering = forceSequentialAnswering;
        QuestionsCount = questionsCount;
        ResultsCount = resultsCount;
        AnyAudios = anyAudios;
        InteractionSettings = interactionSettings;
        ShuffleQuestions = shuffleQuestions;
    }

    public static GeneralVoki CreateNew(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publishedDate,
        GeneralVokiInteractionSettings interactionSettings,
        ushort questionsCount, ushort resultsCount, bool anyAudios,
        bool forceSequentialAnswering, bool shuffleQuestions
    ) {
        GeneralVoki voki = new GeneralVoki(
            id, name, cover, primaryAuthorId, coAuthorIds, managers,
            details, tags, publishedDate, interactionSettings,
            questionsCount: questionsCount, resultsCount: resultsCount, anyAudios: anyAudios,
            forceSequentialAnswering: forceSequentialAnswering, shuffleQuestions: shuffleQuestions
        );
        voki.AddDomainEvent(new PublishedVokiCreatedEvent(
            voki.Id, voki.PrimaryAuthorId, voki.CoAuthorIds, voki.Tags)
        );

        return voki;
    }
}