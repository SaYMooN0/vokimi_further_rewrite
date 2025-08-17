using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.voki_cover;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public sealed class GeneralVoki : AggregateRoot<VokiId>
{
    private GeneralVoki() { }
    private IReadOnlyCollection<VokiQuestion> Questions { get; }
    public bool ForceSequentialAnswering { get; }
    private bool ShuffleQuestions { get; }
    private IReadOnlyCollection<VokiResult> Results { get; }
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
        VokiId id, VokiCoverKey coverKey,
        ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering, bool shuffleQuestions
    ) {
        var voki = new GeneralVoki(id, questions, results, forceSequentialAnswering, shuffleQuestions);
        List<BaseStorageKey> vokiContentKeys = GatherVokiContentKeys(coverKey, questions, results);
        voki.AddDomainEvent(new PublishedVokiCreatedEvent(voki.Id, vokiContentKeys.ToArray()));
        return voki;
    }

    private static List<BaseStorageKey> GatherVokiContentKeys(
        VokiCoverKey coverKey, ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results
    ) {
        List<BaseStorageKey> keys = results
            .Select(r => r.Image)
            .OfType<BaseStorageKey>() //skipping nulls and casting
            .ToList();

        foreach (var question in questions) {
            keys.AddRange(question.Images.Select(k => k as BaseStorageKey));
            var answerKeys = question.Answers
                .Select(a => a.TypeData)
                .OfType<IVokiAnswerTypeDataWithStorageKey>()
                .Select(data => data.Key);
            keys.AddRange(answerKeys);
        }

        keys.Add(coverKey);
        
        return keys;
    }
}