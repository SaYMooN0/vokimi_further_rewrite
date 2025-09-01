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
    protected ImmutableArray<TakingSessionExpectedQuestion> Questions { get; }

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
}