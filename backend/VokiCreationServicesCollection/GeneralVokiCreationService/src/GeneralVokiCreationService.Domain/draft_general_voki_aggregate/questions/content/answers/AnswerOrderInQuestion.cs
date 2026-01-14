namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;

public sealed class AnswerOrderInQuestion : ValueObject
{
    public ushort Value { get; }

    private AnswerOrderInQuestion(ushort value) {
        Value = value;
    }

    public static ErrOr<AnswerOrderInQuestion> Create(ushort value) {
        return new AnswerOrderInQuestion(value);
    }

    public override IEnumerable<object> GetEqualityComponents() => [Value];
}