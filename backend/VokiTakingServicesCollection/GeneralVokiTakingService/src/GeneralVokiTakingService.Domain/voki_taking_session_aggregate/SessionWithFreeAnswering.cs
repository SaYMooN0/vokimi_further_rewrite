namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithFreeAnswering : BaseVokiTakingSession
{
    public override bool IsWithForceSequentialAnswering => false;

    public ImmutableArray<SessionWithFreeAnsweringAnsweredQuestion> AnsweredQuestions { get; private set; }

    public ErrOrNothing SaveAnswers( /*new list*/) {
        throw new NotImplementedException();
        if (VokiTaker is null) {
            return ErrFactory.AuthRequired();
        }

        return ErrOrNothing.Nothing;
    }
}

public record SessionWithFreeAnsweringAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds
);