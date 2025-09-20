using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.contracts;

public record StartTakingResponse(
    string Id,
    string VokiName,
    bool ForceSequentialAnswering,
    GeneralVokiTakingResponseQuestionData[] Questions,
    string SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
)
{
    public static StartTakingResponse Create(StartVokiTakingCommandResponse dto) => new(
        dto.Id.ToString(),
        dto.Name.ToString(),
        dto.ForceSequentialAnswering,
        dto.Questions.Select(GeneralVokiTakingResponseQuestionData.Create).ToArray(),
        dto.SessionId.ToString(),
        dto.StartedAt,
        dto.TotalQuestionsCount
    );
}