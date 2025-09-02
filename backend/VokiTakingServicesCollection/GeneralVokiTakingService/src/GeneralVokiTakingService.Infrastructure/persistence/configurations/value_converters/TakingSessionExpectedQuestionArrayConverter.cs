using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;

internal class TakingSessionExpectedQuestionsArrayConverter :
    ValueConverter<ImmutableArray<TakingSessionExpectedQuestion>, string[]>
{
    public TakingSessionExpectedQuestionsArrayConverter() : base(
        questions => ToStringArray(questions),
        strings => FromStringArray(strings)
    ) { }

    private const char Sep = '|';

    private static string[] ToStringArray(ImmutableArray<TakingSessionExpectedQuestion> questions) =>
        questions.Select(QuestionToString).ToArray();

    private static string QuestionToString(TakingSessionExpectedQuestion q) =>
        $"{q.QuestionId}{Sep}{q.OrderInVokiTaking}{Sep}{q.MinAnswersCount}{Sep}{q.MaxAnswersCount}{Sep}{AnswerIdsToString(q.AnswerIds)}";

    private static string AnswerIdsToString(ImmutableHashSet<GeneralVokiAnswerId> answers) =>
        string.Join(',', answers.Select(a => a.ToString()));

    private static ImmutableArray<TakingSessionExpectedQuestion> FromStringArray(string[] strs) {
        List<TakingSessionExpectedQuestion> questions = new(strs.Length);
        foreach (var str in strs) {
            var parts = str.Split(Sep);
            if (parts.Length != 5) {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    $"Wrong number of parts {strs.Length}"
                ));
            }

            GeneralVokiQuestionId questionId = new(new(parts[0]));
            ushort orderInVokiTaking = ushort.Parse(parts[1]);
            ushort minAnswersCount = ushort.Parse(parts[2]);
            ushort maxAnswersCount = ushort.Parse(parts[3]);
            ImmutableHashSet<GeneralVokiAnswerId> chosenAnswerIds = parts[4]
                .Split(',')
                .Select(aId => new GeneralVokiAnswerId(new(aId)))
                .ToImmutableHashSet();
            questions.Add(new TakingSessionExpectedQuestion(
                questionId, orderInVokiTaking, minAnswersCount, maxAnswersCount, chosenAnswerIds
            ));
        }

        return questions.ToImmutableArray();
    }
}

internal class TakingSessionExpectedQuestionsArrayComparer : ValueComparer<ImmutableArray<TakingSessionExpectedQuestion>>
{
    public TakingSessionExpectedQuestionsArrayComparer() : base(
        (t1, t2) => t1.SequenceEqual(t2!, EqualityComparer<TakingSessionExpectedQuestion>.Default),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableArray()
    ) { }
}