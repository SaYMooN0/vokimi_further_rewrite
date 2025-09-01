using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithFreeAnswering : BaseVokiTakingSession
{
    private SessionWithFreeAnswering() { }

    public override bool IsWithForceSequentialAnswering => false;

    public ImmutableArray<SessionWithFreeAnsweringAnsweredQuestion> AnsweredQuestions { get; private set; }

    public ErrOrNothing SaveAnswers( /*new list*/) {
        throw new NotImplementedException();
        if (VokiTaker is null) {
            return ErrFactory.AuthRequired();
        }

        return ErrOrNothing.Nothing;
    }

    private SessionWithFreeAnswering(
        VokiTakingSessionId id, VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        ImmutableArray<TakingSessionExpectedQuestion> questions
    ) : base(id, vokiId, vokiTaker, startTime, questions) { }

    public static SessionWithFreeAnswering Create(
        VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        IEnumerable<VokiQuestion> vokiQuestions, bool shuffleQuestions
    ) {
        VokiQuestion[] ordered = (
            shuffleQuestions
                ? vokiQuestions.OrderBy(_ => Guid.NewGuid())
                : vokiQuestions.OrderBy(q => q.OrderInVoki)
        ).ToArray();

        List<TakingSessionExpectedQuestion> sessionQuestion = new(ordered.Length);
        for (ushort i = 0; i < ordered.Length; i++) {
            var question = ordered[i];
            sessionQuestion.Add(new TakingSessionExpectedQuestion(
                question.Id,
                OrderInVokiTaking: i,
                question.AnswersCountLimit.MinAnswers,
                question.AnswersCountLimit.MaxAnswers,
                question.Answers.Select(a => a.Id).ToImmutableArray()
            ));
        }

        return new SessionWithFreeAnswering(
            VokiTakingSessionId.CreateNew(), vokiId, vokiTaker, startTime,
            sessionQuestion.ToImmutableArray()
        );
    }
}

public record SessionWithFreeAnsweringAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds
);