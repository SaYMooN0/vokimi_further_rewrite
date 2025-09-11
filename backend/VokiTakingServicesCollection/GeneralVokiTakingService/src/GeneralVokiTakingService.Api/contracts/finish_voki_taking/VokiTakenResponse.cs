using GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

namespace GeneralVokiTakingService.Api.contracts.finish_voki_taking;

public record class VokiTakenResponse(
    VokiResultViewResponse ReceivedResult,
    TimeSpan TimeTaken
)
{
    public static VokiTakenResponse Create(FinishVokiTakingCommandsResult data) => new(
        VokiResultViewResponse.Create(data.ReceivedResult),
        data.TimeTaken
    );
}