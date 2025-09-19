using GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;
using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using SharedKernel.auth;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.exceptions;
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
    private IReadOnlyCollection<VokiResult> _results { get; }
    public ImmutableHashSet<VokiTakenRecordId> VokiTakenRecordIds { get; private set; }
    public GeneralVokiResultsVisibility ResultsVisibility { get; } = GeneralVokiResultsVisibility.Anyone;

    public GeneralVoki(
        VokiId id,
        ImmutableArray<VokiQuestion> questions, ImmutableArray<VokiResult> results,
        bool forceSequentialAnswering, bool shuffleQuestions
    ) {
        Id = id;
        Questions = questions;
        _results = results;
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

    public ErrOr<GeneralVokiResultId> GetResultIdByChosenAnswers(
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers
    ) {
        var questionsById = Questions.ToDictionary(q => q.Id);
        foreach (var providedId in chosenAnswers.Keys) {
            if (!questionsById.ContainsKey(providedId)) {
                return ErrFactory.IncorrectFormat("You answered a question that is not part of this test");
            }
        }

        foreach (var q in Questions) {
            if (
                !chosenAnswers.TryGetValue(q.Id, out var provided)
                || provided is null
                || provided.Count == 0
            ) {
                var preview = string.IsNullOrEmpty(q.Text)
                    ? ""
                    : (q.Text.Length < 30 ? q.Text : q.Text.Substring(0, 25) + " ...");
                var min = q.AnswersCountLimit.MinAnswers;
                var max = q.AnswersCountLimit.MaxAnswers;
                var expectedText = (min == max) ? $"exactly {min} answer(s)" : $"from {min} to {max} answers";
                return ErrFactory.NoValue.Common($"You did not answer the question: \"{preview}\". Choose {expectedText}");
            }

            var count = provided.Count;
            if (count < q.AnswersCountLimit.MinAnswers || count > q.AnswersCountLimit.MaxAnswers) {
                var preview = string.IsNullOrEmpty(q.Text)
                    ? ""
                    : (q.Text.Length < 30 ? q.Text : q.Text.Substring(0, 25) + " ...");
                var min = q.AnswersCountLimit.MinAnswers;
                var max = q.AnswersCountLimit.MaxAnswers;
                var expectedText = (min == max) ? $"exactly {min} answer(s)" : $"from {min} to {max} answers";
                return ErrFactory.ValueOutOfRange($"Incorrect answers count for \"{preview}\". Choose {expectedText}");
            }

            var allowedIds = q.Answers.Select(a => a.Id).ToImmutableHashSet();
            if (!provided.IsSubsetOf(allowedIds)) {
                var preview = string.IsNullOrEmpty(q.Text)
                    ? ""
                    : (q.Text.Length < 30 ? q.Text : q.Text.Substring(0, 25) + " ...");
                return ErrFactory.IncorrectFormat(
                    $"Your selection includes an option that is not available for \"{preview}\". Please select only from the shown options");
            }
        }

        Dictionary<GeneralVokiResultId, int> resultsScore = new();
        foreach (var q in Questions) {
            ImmutableHashSet<GeneralVokiAnswerId> answersForQuestion = chosenAnswers[q.Id];

            var answersById = q.Answers.ToDictionary(a => a.Id);
            foreach (var ansId in answersForQuestion) {
                foreach (var resultId in answersById[ansId].RelatedResultIds) {
                    resultsScore[resultId] = resultsScore.GetValueOrDefault(resultId, 0) + 1;
                }
            }
        }

        if (resultsScore.Count == 0) {
            return ErrFactory.Conflict("Cannot determine result based on chosen answers");
        }

        var bestResultId = resultsScore.MaxBy(kvp => kvp.Value).Key;
        return _results.First(r => r.Id == bestResultId).Id;
    }


    public ErrOr<VokiResult> GetResultToViewByAnyOne(GeneralVokiResultId resultId) {
        if (ResultsVisibility != GeneralVokiResultsVisibility.Anyone) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not show result for anyone, when visibility is set to {GeneralVokiResultsVisibility.Anyone}"
            ));
        }

        var result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return ErrFactory.NotFound.Common("Voki doesn't have requested result");
        }

        return result;
    }

    public ErrOr<VokiResult> GetResultToViewByUserAfterTaking(
        GeneralVokiResultId resultId,ISet<GeneralVokiResultId> userReceivedResultIds
    ) {
        if (ResultsVisibility != GeneralVokiResultsVisibility.AfterTaking) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not show result after taking, when visibility is set to {ResultsVisibility}"
            ));
        }

        var vokiResultIds = _results.Select(r => r.Id);
        bool userHasTaken = userReceivedResultIds.Overlaps(vokiResultIds);
        if (!userHasTaken) {
            return ErrFactory.NoAccess("To see this result you need to take this voki as a logged in user");
        }

        var result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return ErrFactory.NotFound.Common("Voki doesn't have requested result");
        }

        return result;
    }


    public ErrOr<VokiResult> GetOnlyReceivedResultToViewByUser(
        GeneralVokiResultId resultId,ISet<GeneralVokiResultId> userReceivedResultIds
    ) {
        if (ResultsVisibility != GeneralVokiResultsVisibility.OnlyReceived) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not show result only if received, when visibility is set to {ResultsVisibility}"
            ));
        }

        if (!userReceivedResultIds.Contains(resultId)) {
            return ErrFactory.NoAccess("To see this result you need to receive it as a logged in user");
        }

        var result = _results.FirstOrDefault(r => r.Id == resultId);
        if (result is null) {
            return ErrFactory.NotFound.Common("Voki doesn't have requested result");
        }

        return result;
    }
}