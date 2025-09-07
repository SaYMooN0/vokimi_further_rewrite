namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public record  VokiTakenRecordQuestionDetails(
    GeneralVokiQuestionId QuestionId,
    ImmutableArray<GeneralVokiAnswerId> ChosenAnswerIds,
    ushort OrderInVokiTaking
);