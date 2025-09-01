using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;

internal class SequentialTakingAnsweredQuestionsArrayConverter :
    ValueConverter<ImmutableArray<SequentialTakingAnsweredQuestion>, string[]>
{
    public SequentialTakingAnsweredQuestionsArrayConverter() : base(
        questions => ToStringArray(questions),
        strings => FromStringArray(strings)
    ) { }

    private const char Sep = '|';

    private static string[] ToStringArray(ImmutableArray<SequentialTakingAnsweredQuestion> questions) =>
        questions.Select(QuestionToString).ToArray();

    private static string QuestionToString(SequentialTakingAnsweredQuestion q) =>
        $"{q.QuestionId}{Sep}{q.OrderInVokiTaking}{Sep}{QuestionAnswersToString(q.ChosenAnswerIds)}{Sep}{q.ShownAt}{Sep}{q.SubmittedAt}";

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
            ushort orderInVokiTaking = ushort.Parse(parts[1]);
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
    public SequentialTakingAnsweredQuestionsArrayComparer() : base(
        (t1, t2) => t1.SequenceEqual(t2),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableArray()
    ) { }
}