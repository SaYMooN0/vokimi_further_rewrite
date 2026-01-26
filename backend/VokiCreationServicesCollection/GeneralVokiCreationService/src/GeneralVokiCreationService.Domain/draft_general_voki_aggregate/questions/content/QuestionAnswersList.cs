using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;

public sealed class QuestionAnswersList<T> : ValueObject where T : BaseQuestionAnswer
{
    private ImmutableArray<T> Items { get; }

    private QuestionAnswersList(ImmutableArray<T> items) {
        Items = items;
    }


    public static ErrOr<QuestionAnswersList<T>> Create(IEnumerable<T> answers) {
        var answersArray = answers as T[] ?? answers.ToArray();
        if (answersArray.Length > VokiQuestion.MaxAnswersCount) {
            return ErrFactory.LimitExceeded(
                $"Answer count limit exceeded. Maximum allowed answers count is {VokiQuestion.MaxAnswersCount}",
                $"Current answers count is {answersArray.Length}"
            );
        }

        return new QuestionAnswersList<T>([..answersArray]);
    }

    public int Count => Items.Length;
    public static QuestionAnswersList<T> Empty() => new([]);

    public override IEnumerable<object> GetEqualityComponents() => Items;

    public QuestionAnswersList<T> ApplyForEach(Func<T, T> func) =>
        new(Items.Select(func).ToImmutableArray());

    public IEnumerable<TOutput> Select<TOutput>(Func<T, TOutput> func) => Items.Select(func);
    public IEnumerable<BaseQuestionAnswer> AsIEnumerable => Items.Select(a => (BaseQuestionAnswer)a);
    public bool All(Func<T, bool> func) => Items.All(func);
}