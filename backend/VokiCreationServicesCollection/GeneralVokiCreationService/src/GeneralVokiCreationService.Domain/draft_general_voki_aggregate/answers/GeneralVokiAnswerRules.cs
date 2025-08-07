namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;

public static class GeneralVokiAnswerRules
{
    public const int
        TextAnswerMinLength = 5,
        TextAnswerMaxLength = 1000;

    public static ErrOrNothing CheckAnswerTextForErrs(string answer) {
        int len = string.IsNullOrWhiteSpace(answer) ? 0 : answer.Length;

        if (len < TextAnswerMinLength) {
            return ErrFactory.IncorrectFormat(
                "Answer text is too short",
                $"Answer must be at least {TextAnswerMinLength} characters long"
            );
        }

        if (len > TextAnswerMaxLength) {
            return ErrFactory.IncorrectFormat(
                "Answer text is too long",
                $"Answer must not exceed {TextAnswerMaxLength} characters"
            );
        }

        return ErrOrNothing.Nothing;
    }
}