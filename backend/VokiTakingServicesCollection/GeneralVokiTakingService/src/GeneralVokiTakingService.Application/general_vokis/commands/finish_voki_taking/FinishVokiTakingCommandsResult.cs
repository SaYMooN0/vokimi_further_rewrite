using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public record class FinishVokiTakingCommandsResult(
    VokiResult ReceivedResult,
    TimeSpan TimeTaken
);