using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.concrete_keys;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public sealed class GeneralVoki : AggregateRoot<VokiId>
{
    private GeneralVoki() { }
    public IReadOnlyCollection<VokiQuestion> Questions { get; }
    public bool ForceSequentialAnswering { get; }
    public bool ShuffleQuestions { get; }
    private IReadOnlyCollection<VokiResult> Results { get; }
    public ImmutableHashSet<VokiTakenRecordId> VokiTakenRecordIds { get; private set; }

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
            keys.AddRange(question.ImageSet.Keys.Select(k => k as BaseStorageKey));
            var answerKeys = question.Answers
                .Select(a => a.TypeData)
                .OfType<IVokiAnswerTypeDataWithStorageKey>()
                .Select(data => data.Key);
            keys.AddRange(answerKeys);
        }

        keys.Add(coverKey);

        return keys;
    }

    public ErrOr<VokiTakingFinishedSuccessfullyData> FinishVokiTaking(
        DateTime startTime,
        DateTime finishTime,
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers,
        AppUserId? vokiTakerId
    ) {
        AddDomainEvent(new GeneralVokiTakenEvent(Id, vokiTakerId));
        throw new NotImplementedException();
    }
}