using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Application.general_vokis.queries;

namespace GeneralVokiTakingService.Api.contracts.voki_taking;

public record ExistingActiveSessionCheckResponse(
    bool DoesActiveSessionExist,
    ExistingActiveSessionCheckResponse.ActiveSessionDataResponse? SessionData
) : ICreatableResponse<CheckIfUserHasActiveSessionForVokiQueryResult>
{
    public record ActiveSessionDataResponse(
        string VokiId,
        string SessionId,
        DateTime StartedAt,
        ushort QuestionsWithSavedAnswersCount,
        ushort TotalQuestionsCount
    ) : IExistingActiveSessionResponse

    {
        public static ActiveSessionDataResponse Create(SavedActiveVokiTakingSessionDto dto) => new(
            dto.VokiId.ToString(),
            dto.SessionId.ToString(),
            dto.StartedAt,
            dto.QuestionsWithSavedAnswersCount,
            dto.TotalQuestionsCount
        );
    }

    public static ICreatableResponse<CheckIfUserHasActiveSessionForVokiQueryResult> Create(
        CheckIfUserHasActiveSessionForVokiQueryResult res
    ) => res.Match<ExistingActiveSessionCheckResponse>(
        sessionExistsFunc: s => new(
            DoesActiveSessionExist: true,
            SessionData: ActiveSessionDataResponse.Create(s)),
        noSessionFunc: () => new(false, null)
    );
}