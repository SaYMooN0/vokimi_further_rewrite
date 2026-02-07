using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.continue_taking;

public record ContinueVokiTakingResponse(
    string VokiId,
    string VokiName,
    bool IsWithForceSequentialAnswering,
    GeneralVokiTakingResponseQuestionData[] Questions,
    string SessionId,
    ushort TotalQuestionsCount,
    Dictionary<string, string[]> ChosenAnswers,
    string CurrentQuestionId
) : ICreatableResponse<ContinueVokiTakingCommandResult>
{
    public static ICreatableResponse<ContinueVokiTakingCommandResult> Create(ContinueVokiTakingCommandResult res) =>
        new ContinueVokiTakingResponse(
            res.SessionData.VokiId.ToString(),
            res.SessionData.VokiName.ToString(),
            res.SessionData.IsWithForceSequentialAnswering,
            res.SessionData.QuestionsToShow.Select(GeneralVokiTakingResponseQuestionData.FromQuestion).ToArray(),
            res.SessionData.SessionId.ToString(),
            res.SessionData.TotalQuestionsCount,
            res.SavedChosenAnswers.ToDictionary(
                qToAnsw => qToAnsw.Key.ToString(),
                qToAnsw => qToAnsw.Value.Select(a => a.ToString()).ToArray()
            ),
            res.CurrentQuestionId.ToString()
        );
}