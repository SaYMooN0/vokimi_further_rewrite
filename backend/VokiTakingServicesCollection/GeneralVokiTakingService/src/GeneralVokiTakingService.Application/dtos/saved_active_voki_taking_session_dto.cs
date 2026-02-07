using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.dtos;

public record SavedActiveVokiTakingSessionDto(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort QuestionsWithSavedAnswersCount,
    ushort TotalQuestionsCount
)
{
    public static SavedActiveVokiTakingSessionDto Create(BaseVokiTakingSession takingSession) => new(
        takingSession.VokiId,
        takingSession.Id,
        takingSession.StartTime,
        takingSession.QuestionsWithSavedAnswersCount(),
        takingSession.TotalQuestionsCount
    );
}