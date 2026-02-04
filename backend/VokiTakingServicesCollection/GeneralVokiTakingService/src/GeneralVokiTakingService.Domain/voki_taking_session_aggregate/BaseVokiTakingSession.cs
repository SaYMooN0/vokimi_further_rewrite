using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using SharedKernel.exceptions;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public abstract class BaseVokiTakingSession : AggregateRoot<VokiTakingSessionId>
{
    protected BaseVokiTakingSession() { }
    public VokiId VokiId { get; }
    public AppUserId? VokiTaker { get; }
    public DateTime StartTime { get; }
    public abstract bool IsWithForceSequentialAnswering { get; }
    public ImmutableArray<TakingSessionExpectedQuestion> Questions { get; }

    protected BaseVokiTakingSession(
        VokiTakingSessionId vokiTakingSessionId,
        VokiId vokiId,
        AppUserId? vokiTaker,
        DateTime startTime,
        ImmutableArray<TakingSessionExpectedQuestion> questions
    ) {
        if (questions.IsDefaultOrEmpty) {
            UnexpectedBehaviourException.ThrowErr(
                ErrFactory.ValueOutOfRange("Session cannot be created without questions")
            );
        }

        Id = vokiTakingSessionId;
        VokiId = vokiId;
        VokiTaker = vokiTaker;
        StartTime = startTime;
        Questions = questions;
    }

    public abstract ImmutableDictionary<GeneralVokiQuestionId, QuestionOrderInVokiTakingSession> QuestionsToShowOnStart();

    public record VokiTakingStateToContinueFromSaved(
        GeneralVokiQuestionId CurrentQuestionId,
        ImmutableDictionary<
            GeneralVokiQuestionId,
            (QuestionOrderInVokiTakingSession Order, ImmutableHashSet<GeneralVokiAnswerId> SavedAnsweres)
        > QuestionsToShow
    );
    public abstract int QuestionsWithSavedAnswersCount();

    public abstract VokiTakingStateToContinueFromSaved GetSavedStateToContinueTaking();

    protected ErrOrNothing ValidateStartAndFinishTime(
        DateTime currentTime,
        ClientServerTimePairDto sessionStartTime,
        DateTime clientFinishTime
    ) {
        if (StartTime > currentTime) {
            return ErrFactory.Conflict("Server session start time cannot be in the future");
        }

        if (sessionStartTime.Server > currentTime) {
            return ErrFactory.Conflict("Provided server start time cannot be in the future");
        }

        if (sessionStartTime.Client > currentTime) {
            return ErrFactory.Conflict("Client start time cannot be in the future");
        }

        if (clientFinishTime > currentTime) {
            return ErrFactory.Conflict("Client finish time cannot be in the future");
        }

        var serverStartTimeDelta = (sessionStartTime.Server - StartTime).Duration();
        if (serverStartTimeDelta > TimeSpan.FromMilliseconds(1)) {
            return ErrFactory.Conflict("Provided server start time does not match the session start time");
        }

        if (TimeSpan.FromTicks(Math.Abs((sessionStartTime.Client - StartTime).Ticks)) > ServerStartTolerance) {
            return ErrFactory.Conflict(
                $"Client start time differs from server start time by more than {ServerStartTolerance.Minutes} minutes");
        }

        if (TimeSpan.FromTicks(Math.Abs((currentTime - clientFinishTime).Ticks)) > ClientFinishTolerance) {
            return ErrFactory.Conflict(
                $"Client finish time differs from current time by more than {ClientFinishTolerance.Minutes} minutes");
        }

        if (sessionStartTime.Client > clientFinishTime) {
            return ErrFactory.Conflict("Start time must be earlier than or equal to client finish time");
        }

        return ErrOrNothing.Nothing;
    }

    protected static ErrOrNothing ValidateSingleQuestionAnswers(
        TakingSessionExpectedQuestion question,
        ImmutableHashSet<GeneralVokiAnswerId>? providedSet
    ) {
        ErrOrNothing errs = ErrOrNothing.Nothing;
        string expectedText = question.MinAnswersCount == question.MaxAnswersCount
            ? $"exactly {question.MinAnswersCount} answer(s)"
            : $"from {question.MinAnswersCount} to {question.MaxAnswersCount} answers";

        if (providedSet is null || providedSet.Count == 0) {
            errs.AddNext(ErrFactory.NoValue.Common(
                $"You did not answer question {question.OrderInVokiTaking.Value}. Choose {expectedText}"
            ));
            return errs;
        }

        var count = providedSet.Count;
        if (count < question.MinAnswersCount || count > question.MaxAnswersCount) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Question {question.OrderInVokiTaking.Value}: choose {expectedText}"
            ));
        }

        if (!providedSet.IsSubsetOf(question.AnswerIds)) {
            errs.AddNext(ErrFactory.IncorrectFormat(
                $"Question {question.OrderInVokiTaking.Value}: some selected answers are not available. Please select only from the shown options"
            ));
        }

        return errs;
    }

    protected ErrOrNothing ValidateVokiTaker(
        IUserCtx userCtx,
        out AppUserId? resolvedVokiTaker
    ) {
        AppUserId? contextId = userCtx.TryGetUserId.IsSuccess(out var id) ? id : null;
        if (
            this.VokiTaker is not null
            && contextId is not null
            && contextId != this.VokiTaker
        ) {
            resolvedVokiTaker = null;
            return ErrFactory.Conflict("Could not finish voki taking because it was started by another user");
        }

        resolvedVokiTaker = contextId ?? this.VokiTaker;

        return ErrOrNothing.Nothing;
    }


    private static readonly TimeSpan ServerStartTolerance = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan ClientFinishTolerance = TimeSpan.FromMinutes(5);

}