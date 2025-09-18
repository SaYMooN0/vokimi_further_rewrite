using GeneralVokiTakingService.Domain.common;

namespace GeneralVokiTakingService.Api.contracts.finish_voki_taking;

public class FinishVokiTakingWithFreeAnsweringRequest : IRequestWithValidationNeeded
{
    public Dictionary<string, string[]> ChosenAnswers { get; init; }
    public DateTime ClientStartTime { get; init; }
    public DateTime ServerStartTime { get; init; }
    public DateTime ClientFinishTime { get; init; }
    public string SessionId { get; init; }

    private const int
        MaxQuestionsCount = 500,
        MaxAnswersInQuestionCount = 500;

    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SessionId) || !Guid.TryParse(SessionId, out var sessionGuid)) {
            return ErrFactory.IncorrectFormat("Provided session id is invalid");
        }

        ParsedSessionId = new VokiTakingSessionId(sessionGuid);

        var nowUtc = DateTime.UtcNow;
        if (ClientFinishTime < ClientStartTime) {
            return ErrFactory.Conflict("Client finish time cannot be earlier than client start time");
        }

        if (ClientStartTime > nowUtc) {
            return ErrFactory.Conflict("Client start time cannot be in the future");
        }

        if (ClientFinishTime > nowUtc) {
            return ErrFactory.Conflict("Client finish time cannot be in the future");
        }

        if (ServerStartTime > nowUtc) {
            return ErrFactory.Conflict("Server start time cannot be in the future");
        }

        if (ChosenAnswers.Count > MaxQuestionsCount) {
            return ErrFactory.ValueOutOfRange();
        }

        ParsedChosenAnswers = new(ChosenAnswers.Count);

        foreach (var (questionIdStr, answersArray) in ChosenAnswers) {
            if (string.IsNullOrWhiteSpace(questionIdStr) || !Guid.TryParse(questionIdStr, out var questionGuid)) {
                return ErrFactory.IncorrectFormat("One or more of provided question id is invalid");
            }

            if (answersArray is null) {
                return ErrFactory.NoValue.Common();
            }

            if (answersArray.Length > MaxAnswersInQuestionCount) {
                return ErrFactory.ValueOutOfRange();
            }

            var parsedAnswers = answersArray
                .Where(id => Guid.TryParse(id, out _))
                .Select(g => new GeneralVokiAnswerId(new(g)))
                .ToImmutableHashSet();


            ParsedChosenAnswers.Add(new(questionGuid), parsedAnswers);
        }

        return ErrOrNothing.Nothing;
    }

    public VokiTakingSessionId ParsedSessionId { get; private set; }
    public Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ParsedChosenAnswers { get; private set; }
}