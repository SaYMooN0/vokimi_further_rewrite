using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.sequential_answering;

public class AnswerQuestionForSequentialAnsweringSessionRequest : IRequestWithValidationNeeded
{
    public string SessionId { get; init; }
    public string QuestionId { get; init; }
    public ushort QuestionOrderInVokiTaking { get; init; }
    public string[] ChosenAnswers { get; init; }
    public DateTime ServerQuestionShownAt { get; init; }
    public DateTime ClientQuestionShownAt { get; init; }
    public DateTime ClientQuestionAnsweredAt { get; init; }
    private const int MaxAnswersInQuestionCount = 500;

    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SessionId) || !Guid.TryParse(SessionId, out var sessionGuid)) {
            return ErrFactory.IncorrectFormat("Provided session id is invalid");
        }

        ParsedSessionId = new VokiTakingSessionId(sessionGuid);

        if (string.IsNullOrWhiteSpace(QuestionId) || !Guid.TryParse(QuestionId, out var questionGuid)) {
            return ErrFactory.IncorrectFormat("Provided question id is invalid");
        }

        ParsedQuestionId = new GeneralVokiQuestionId(questionGuid);

        if (ChosenAnswers is null) {
            return ErrFactory.NoValue.Common("Chosen answers are missing");
        }


        if (ChosenAnswers is null) {
            return ErrFactory.NoValue.Common("Answers array is null");
        }

        if (ChosenAnswers.Length > MaxAnswersInQuestionCount) {
            return ErrFactory.ValueOutOfRange(
                "Too many answers provided for the question",
                $"Maximum allowed is {MaxAnswersInQuestionCount}"
            );
        }

        ImmutableHashSet<GeneralVokiAnswerId> parsedAnswers = ChosenAnswers
            .Where(id => Guid.TryParse(id, out _))
            .Select(id => new GeneralVokiAnswerId(new Guid(id)))
            .ToImmutableHashSet();

        if (parsedAnswers.Count < ChosenAnswers.Length) {
            return ErrFactory.IncorrectFormat(
                $"Couldn't parse some of the chosen answers. Chosen: {ChosenAnswers.Length}. Parsed: {parsedAnswers.Count}",
                "Try reloading the page"
            );
        }

        ParsedChosenAnswers = parsedAnswers;


        if (ClientQuestionAnsweredAt < ClientQuestionShownAt) {
            return ErrFactory.Conflict("Client answered before the question was shown");
        }

        return ErrOrNothing.Nothing;
    }

    public VokiTakingSessionId ParsedSessionId { get; private set; }
    public GeneralVokiQuestionId ParsedQuestionId { get; private set; }
    public ImmutableHashSet<GeneralVokiAnswerId> ParsedChosenAnswers { get; private set; }
    public ClientServerTimePairDto ParsedShownAt => new(Client: ClientQuestionShownAt, Server: ServerQuestionShownAt);
}