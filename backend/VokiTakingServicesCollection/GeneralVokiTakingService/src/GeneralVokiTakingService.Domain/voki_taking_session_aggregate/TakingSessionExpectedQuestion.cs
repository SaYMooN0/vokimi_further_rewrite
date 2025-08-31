namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public record TakingSessionExpectedQuestion(
    GeneralVokiQuestionId QuestionId,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    ImmutableArray<GeneralVokiAnswerId> AnswerIds
);