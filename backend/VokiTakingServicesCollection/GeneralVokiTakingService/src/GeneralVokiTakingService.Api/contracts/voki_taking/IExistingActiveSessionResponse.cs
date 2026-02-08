namespace GeneralVokiTakingService.Api.contracts.voki_taking;

public interface IExistingActiveSessionResponse
{
    public string VokiId { get; }
    public string SessionId { get; }
    public DateTime StartedAt { get; }
    public ushort QuestionsWithSavedAnswersCount { get; }
    public ushort TotalQuestionsCount { get; }
}