using GeneralVokiTakingService.Domain.common;
using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public abstract class BaseVokiTakingSession : AggregateRoot<VokiTakingSessionId>
{
    protected BaseVokiTakingSession() { }

    public VokiId VokiId { get; private set; }
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

    public ImmutableDictionary<GeneralVokiQuestionId, ushort> QuestionIdToOrder() => Questions.ToImmutableDictionary(
        q => q.QuestionId,
        q => q.OrderInVokiTaking
    );


    public ErrOrNothing ValidateTime(
        DateTime currentTime,
        DateTime clientStartTime,
        DateTime providedServerStartTime,
        DateTime clientFinishTime
    ) {
        if (StartTime > currentTime) {
            return ErrFactory.Conflict("Server session start time cannot be in the future");
        }

        if (providedServerStartTime > currentTime) {
            return ErrFactory.Conflict("Provided server start time cannot be in the future");
        }

        if (clientStartTime > currentTime) {
            return ErrFactory.Conflict("Client start time cannot be in the future");
        }

        if (clientFinishTime > currentTime) {
            return ErrFactory.Conflict("Client finish time cannot be in the future");
        }

        if (providedServerStartTime != StartTime) {
            return ErrFactory.Conflict("Provided server start time does not match the session start time");
        }

        if (TimeSpan.FromTicks(Math.Abs((clientStartTime - StartTime).Ticks)) > ServerStartTolerance) {
            return ErrFactory.Conflict(
                $"Client start time differs from server start time by more than {ServerStartTolerance.Minutes} minutes");
        }

        if (TimeSpan.FromTicks(Math.Abs((currentTime - clientFinishTime).Ticks)) > ClientFinishTolerance) {
            return ErrFactory.Conflict(
                $"Client finish time differs from current time by more than {ClientFinishTolerance.Minutes} minutes");
        }

        if (clientStartTime > clientFinishTime) {
            return ErrFactory.Conflict("Start time must be earlier than or equal to client finish time");
        }

        return ErrOrNothing.Nothing;
    }

    private static readonly TimeSpan ServerStartTolerance = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan ClientFinishTolerance = TimeSpan.FromMinutes(5);
}