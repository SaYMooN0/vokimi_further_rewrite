using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.contracts;

public record GeneralVokiTakingResponse(
    string Id,
    bool ForceSequentialAnswering,
    GeneralVokiTakingResponseQuestionData[] Questions,
    string SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
)
{
    public static GeneralVokiTakingResponse Create(StartVokiTakingCommandResponse commandResponse) => new(
        commandResponse.Id.ToString(),
        commandResponse.ForceSequentialAnswering,
        commandResponse.Questions.Select(GeneralVokiTakingResponseQuestionData.Create).ToArray(),
        commandResponse.SessionId.ToString(),
        commandResponse.StartedAt,
        commandResponse.TotalQuestionsCount
    );
}