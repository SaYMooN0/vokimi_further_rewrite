using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithSequentialAnswering : BaseVokiTakingSession
{
    private SessionWithSequentialAnswering() { }
    public override bool IsWithForceSequentialAnswering => true;
    public ushort CurrentQuestionOrder { get; private set; }
    private ImmutableArray<SequentialTakingAnsweredQuestion> _answered { get; set; }

    private SessionWithSequentialAnswering(
        VokiTakingSessionId id, VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        ImmutableArray<TakingSessionExpectedQuestion> questions,
        ushort currentQuestionOrder
    ) : base(id, vokiId, vokiTaker, startTime, questions) {
        CurrentQuestionOrder = currentQuestionOrder;
    }

    public static SessionWithSequentialAnswering Create(
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
                question.Answers.Select(a => a.Id).ToImmutableHashSet()
            ));
        }

        return new SessionWithSequentialAnswering(
            VokiTakingSessionId.CreateNew(), vokiId, vokiTaker, startTime,
            questions: sessionQuestion.ToImmutableArray(),
            currentQuestionOrder: 0
        );
    }

    public TakingSessionExpectedQuestion GetCurrentQuestion() {
        TakingSessionExpectedQuestion? current = Questions
            .FirstOrDefault(q => q.OrderInVokiTaking == CurrentQuestionOrder);

        if (current is null) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.ValueOutOfRange("No unanswered questions left"));
        }

        return current;
    }

    public ImmutableArray<VokiTakenQuestionDetails> GatherQuestionDetails() {
        //check thal all questions are answered
        throw new NotImplementedException();
        return _answered
            .Select(q => new VokiTakenQuestionDetails(
                q.QuestionId,
                q.ChosenAnswerIds,
                q.OrderInVokiTaking
            ))
            .ToImmutableArray();
    }

    public ErrOrNothing MarkQuestionAsAnswered(SequentialTakingAnsweredQuestion data) {
        var expected = Questions.FirstOrDefault(q => q.QuestionId == data.QuestionId);
        if (expected is null) {
            return ErrFactory.NotFound.Common(
                "This question is not expected or has already been answered",
                details: "Refresh the page. If the issue persists, restart the attempt"
            );
        }

        if (_answered.Any(a => a.QuestionId == expected.QuestionId)) {
            return ErrFactory.Conflict(
                "This question has already been answered",
                "Please reload the page. If needed, contact support about changing answers."
            );
        }

        if (expected.OrderInVokiTaking != data.OrderInVokiTaking) {
            return ErrFactory.Conflict(
                "Question order mismatch",
                $"Expected order {expected.OrderInVokiTaking}, got {data.OrderInVokiTaking}. Please reload the page and continue with the current question"
            );
        }

        var current = Questions
            .Where(q => !_answered.Any(a => a.QuestionId == q.QuestionId))
            .OrderBy(q => q.OrderInVokiTaking)
            .FirstOrDefault();

        if (current is null) {
            return ErrFactory.Conflict(
                "There are no questions left to answer",
                "Please reload the page to finalize the attempt"
            );
        }

        if (current.QuestionId != data.QuestionId) {
            return ErrFactory.Conflict(
                "You cannot answer this question yet",
                $"Sequential mode is enabled. Please answer the current question (order {current.OrderInVokiTaking}) first"
            );
        }

        if (data.ShownAt < StartTime) {
            return ErrFactory.Conflict(
                "Time sync error: question shown before the voki taking started.",
                "Please reload the page and try again"
            );
        }

        if (data.SubmittedAt < data.ShownAt) {
            return ErrFactory.Conflict(
                "Time sync error: submitted earlier than shown.",
                "Please reload the page and try again"
            );
        }

        var chosenCount = data.ChosenAnswerIds.Count;

        if (chosenCount < expected.MinAnswersCount) {
            return ErrFactory.Conflict(
                "Too few answers selected",
                $"Please select more option(s) to meet the required range from {expected.MinAnswersCount} to {expected.MaxAnswersCount}"
            );
        }

        if (chosenCount > expected.MaxAnswersCount) {
            return ErrFactory.Conflict(
                "Too many answers selected",
                $"Please remove extra option(s) to meet the required range {expected.MinAnswersCount} to {expected.MaxAnswersCount}"
            );
        }

        var allowed = expected.AnswerIds.ToImmutableHashSet();
        var invalidChosen = data.ChosenAnswerIds.Where(a => !allowed.Contains(a)).ToArray();
        if (invalidChosen.Length > 0) {
            return ErrFactory.Conflict(
                "Some selected options are not allowed for this question",
                "Please clear your selection and choose again from the provided list"
            );
        }

        _answered = _answered.Add(data);

        var next = Questions
            .Where(q => !_answered.Any(a => a.QuestionId == q.QuestionId))
            .OrderBy(q => q.OrderInVokiTaking)
            .FirstOrDefault();

        if (next is not null) {
            CurrentQuestionOrder = next.OrderInVokiTaking;
        }

        return ErrOrNothing.Nothing;
    }
}