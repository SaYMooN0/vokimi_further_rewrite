namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public record SequentialTakingAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ushort OrderInVokiTaking,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds,
    DateTime ShownAt,
    DateTime SubmittedAt
);