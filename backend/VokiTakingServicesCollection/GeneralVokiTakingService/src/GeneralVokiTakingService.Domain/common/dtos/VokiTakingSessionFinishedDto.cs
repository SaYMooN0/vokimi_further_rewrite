using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.common.dtos;

public record class VokiTakingSessionFinishedDto(
    VokiId TakenVokiId,
    AppUserId? VokiTakerId,
    DateTime SessionStartTime,
    DateTime SessionFinishTime,
    bool WasSessionWithForcedSequentialOrder,
    GeneralVokiResultId ReceivedResultId,
    ImmutableArray<VokiTakenQuestionDetails> QuestionDetails
) { }