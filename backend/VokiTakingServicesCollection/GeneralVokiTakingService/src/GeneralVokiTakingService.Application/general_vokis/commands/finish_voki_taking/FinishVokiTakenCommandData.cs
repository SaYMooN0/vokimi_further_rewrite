using GeneralVokiTakingService.Domain.common;

namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public record FinishVokiTakenCommandData(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    Dictionary<GeneralVokiQuestionId, HashSet<GeneralVokiAnswerId>> ChosenAnswers,
    DateTime ClientStartTime,
    DateTime ServerStartTime,
    DateTime FinishTime
);