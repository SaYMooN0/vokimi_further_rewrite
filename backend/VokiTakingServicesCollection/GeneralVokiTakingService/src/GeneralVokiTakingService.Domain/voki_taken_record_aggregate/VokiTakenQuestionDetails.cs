namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public record VokiTakenQuestionDetails(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds,
    ushort OrderInVokiTaking
);