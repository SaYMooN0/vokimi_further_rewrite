namespace GeneralVokiTakingService.Domain.common;

public record  VokiTakenQuestionDetails(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds,
    ushort OrderInVokiTaking
);