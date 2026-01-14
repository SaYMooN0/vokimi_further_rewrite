using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;

public sealed class QuestionAnswersList<T> : ValueObject where T : BaseQuestionAnswer
{
    public ImmutableArray<T> Items { get; }

    private QuestionAnswersList(ImmutableArray<T> items) {
        Items = items;
    }


    public static ErrOr<QuestionAnswersList<T>> Create(ImmutableArray<T> answers) {
        if (answers.Length > VokiQuestion.MaxAnswersCount) {
            return ErrFactory.LimitExceeded(
                $"Answer count limit exceeded. Maximum allowed answers count is {VokiQuestion.MaxAnswersCount}",
                $"Current answers count is {answers.Length}"
            );
        }

        return new QuestionAnswersList<T>(answers);
    }

    public int Count => Items.Length;
    public static QuestionAnswersList<T> Empty() => new([]);

    public override IEnumerable<object> GetEqualityComponents() => Items;
}