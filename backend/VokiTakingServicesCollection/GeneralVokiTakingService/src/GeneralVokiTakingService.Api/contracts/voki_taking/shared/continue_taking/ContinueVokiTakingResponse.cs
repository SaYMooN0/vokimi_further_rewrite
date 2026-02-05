using GeneralVokiTakingService.Application.common.dtos;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.continue_taking;

public record ContinueVokiTakingResponse(
    string VokiId,
    string VokiName,
    bool IsWithForceSequentialAnswering,
    GeneralVokiTakingResponseQuestionData[] Questions,
    string SessionId,
    ushort TotalQuestionsCount,
    Dictionary<string, string[]> ChosenAnswers
) : ICreatableResponse<VokiTakingData>
{
    public static ICreatableResponse<VokiTakingData> Create(VokiTakingData success) => ;
}