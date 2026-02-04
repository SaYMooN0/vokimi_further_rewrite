using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.voki_taking_sessions;

internal class SequentialTakingAnsweredQuestionsArrayConverter :
    ValueConverter<ImmutableArray<SequentialTakingAnsweredQuestion>, string[]>
{
    public SequentialTakingAnsweredQuestionsArrayConverter() : base(
        questions => ToStringArray(questions),
        strings => FromStringArray(strings)
    ) { }

    private const char Sep = '|';

    private static string[] ToStringArray(ImmutableArray<SequentialTakingAnsweredQuestion> questions) =>
        questions.IsDefaultOrEmpty ? [] : questions.Select(QuestionToString).ToArray();

    private static string QuestionToString(SequentialTakingAnsweredQuestion q) =>
        $"{q.QuestionId}{Sep}{q.OrderInVokiTaking}{Sep}{QuestionAnswersToString(q.ChosenAnswerIds)}{Sep}{q.ClientShownAt}{Sep}{q.ClientSubmittedAt}";

    private static string QuestionAnswersToString(ImmutableHashSet<GeneralVokiAnswerId> answers) =>
        string.Join(',', answers.Select(a => a.ToString()));

    private static ImmutableArray<SequentialTakingAnsweredQuestion> FromStringArray(string[] strs) {
        List<SequentialTakingAnsweredQuestion> questions = new(strs.Length);
        foreach (var str in strs) {
            var parts = str.Split(Sep);
            if (parts.Length != 5) {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    $"Wrong number of parts {strs.Length}"
                ));
            }

            GeneralVokiQuestionId questionId = new(new(parts[0]));
            var orderInVokiTaking = QuestionOrderInVokiTakingSession.Create(ushort.Parse(parts[1])).AsSuccess();
            ImmutableHashSet<GeneralVokiAnswerId> chosenAnswerIds = parts[2]
                .Split(',')
                .Select(aId => new GeneralVokiAnswerId(new(aId)))
                .ToImmutableHashSet();
            DateTime shownAt = DateTime.Parse(parts[3]);
            DateTime submittedAt = DateTime.Parse(parts[4]);
            questions.Add(new SequentialTakingAnsweredQuestion(
                questionId, orderInVokiTaking, chosenAnswerIds, shownAt, submittedAt
            ));
        }

        return questions.ToImmutableArray();
    }
}

internal class SequentialTakingAnsweredQuestionsArrayComparer : ValueComparer<ImmutableArray<SequentialTakingAnsweredQuestion>>
{
    public SequentialTakingAnsweredQuestionsArrayComparer()
        : base(
            (a, b) => AreEqual(a, b),
            (a) => GetHashCodeSafe(a),
            (a) => SnapshotArray(a)
        ) { }

    private static bool AreEqual(
        ImmutableArray<SequentialTakingAnsweredQuestion> t1, ImmutableArray<SequentialTakingAnsweredQuestion> t2
    ) {
        if (t1.IsDefault && t2.IsDefault) {
            return true;
        }

        if (t1.IsDefault || t2.IsDefault) {
            return false;
        }

        return t1.SequenceEqual(t2, EqualityComparer<SequentialTakingAnsweredQuestion>.Default);
    }

    private static int GetHashCodeSafe(ImmutableArray<SequentialTakingAnsweredQuestion> t) {
        if (t.IsDefaultOrEmpty)
            return 0;

        int hash = 17;
        foreach (var x in t) {
            hash = hash * 31 + (x is null ? 0 : x.GetHashCode());
        }

        return hash;
    }

    private static ImmutableArray<SequentialTakingAnsweredQuestion> SnapshotArray(
        ImmutableArray<SequentialTakingAnsweredQuestion> t
    ) => t.IsDefault
        ? ImmutableArray<SequentialTakingAnsweredQuestion>.Empty
        : [..t];
}