namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class QuestionAnswersCountLimit : ValueObject
{
    private const ushort MinPossibleAnswersLimit = 1;
    public bool IsMultipleChoice { get; }
    public ushort MinAnswers { get; }
    public ushort MaxAnswers { get; }

    private QuestionAnswersCountLimit(bool isMultipleChoice, ushort minAnswers, ushort maxAnswers) {
        IsMultipleChoice = isMultipleChoice;
        MinAnswers = minAnswers;
        MaxAnswers = maxAnswers;
    }

    public static QuestionAnswersCountLimit SingleChoice() => new(false, 1, 1);

    public static ErrOr<QuestionAnswersCountLimit> MultipleChoice(ushort minAnswers, ushort maxAnswers) {
        if (minAnswers < MinPossibleAnswersLimit) {
            return ErrFactory.IncorrectFormat(
                "Minimum answers value is less than allowed",
                $"Provided value '{minAnswers}' is less than the minimum allowed of {MinPossibleAnswersLimit}"
            );
        }

        if (minAnswers > maxAnswers) {
            return ErrFactory.Conflict(
                "Invalid range for answers count limit. Minimum answers value is greater than the maximum answers value",
                $"Minimum answers '{minAnswers}' cannot be greater than maximum answers '{maxAnswers}'"
            );
        }

        return new QuestionAnswersCountLimit(true, minAnswers, maxAnswers);
    }

    public override IEnumerable<object> GetEqualityComponents() => [IsMultipleChoice, MinAnswers, MaxAnswers];
}