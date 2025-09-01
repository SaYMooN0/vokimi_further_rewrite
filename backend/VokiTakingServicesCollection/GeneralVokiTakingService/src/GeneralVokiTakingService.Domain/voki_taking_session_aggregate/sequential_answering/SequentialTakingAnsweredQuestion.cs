namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;

public record SequentialTakingAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ushort OrderInVokiTaking,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds,
    DateTime ShownAt,
    DateTime SubmittedAt
);