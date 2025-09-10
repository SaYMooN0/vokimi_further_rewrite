using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.common;

public record VokiTakingFinishedSuccessfullyData(
    VokiId TakenVokiId,
    AppUserId? VokiTakerId,
    DateTime TestTakingStart,
    DateTime TestTakingEnd,
    GeneralVokiResultId ReceivedResultId,
    ImmutableArray<VokiTakenQuestionDetails> QuestionDetails,
    bool WasVokiWithForcedSequentialOrder
) { }