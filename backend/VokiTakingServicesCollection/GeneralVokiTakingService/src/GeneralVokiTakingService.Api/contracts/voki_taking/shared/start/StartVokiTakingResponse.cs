using System.Runtime.CompilerServices;
using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.start;

public abstract record StartVokiTakingResponse(
    string VokiId,
    string SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
) : ICreatableResponse<IStartVokiTakingCommandResult>
{
    public abstract bool NewSessionStarted { get; }

    public static ICreatableResponse<IStartVokiTakingCommandResult> Create(IStartVokiTakingCommandResult res) => res switch {
        SuccessStartVokiTakingCommandResult success => VokiTakingSuccessfullyStartedResponse.Create(success),
        StartVokiTakingCommandActiveSessionExistsResult active => StartVokiTakingActiveSessionExistsResponse.Create(active),
        _ => throw new SwitchExpressionException(res),
    };

    public sealed record VokiTakingSuccessfullyStartedResponse(
        string VokiId,
        string VokiName,
        bool IsWithForceSequentialAnswering,
        GeneralVokiTakingResponseQuestionData[] Questions,
        string SessionId,
        DateTime StartedAt,
        ushort TotalQuestionsCount
    ) : StartVokiTakingResponse(VokiId, SessionId, StartedAt, TotalQuestionsCount)
    {
        public override bool NewSessionStarted => true;

        public static VokiTakingSuccessfullyStartedResponse Create(SuccessStartVokiTakingCommandResult res) => new(
            res.Data.VokiId.ToString(),
            res.Data.Name.ToString(),
            res.Data.IsWithForceSequentialAnswering,
            res.Data.Questions.Select(GeneralVokiTakingResponseQuestionData.FromQuestion).ToArray(),
            res.Data.SessionId.ToString(),
            res.Data.StartedAt,
            res.Data.TotalQuestionsCount
        );
    }

    public sealed record StartVokiTakingActiveSessionExistsResponse(
        string VokiId,
        string SessionId,
        DateTime StartedAt,
        ushort QuestionsWithSavedAnswersCount,
        ushort TotalQuestionsCount
    ) : StartVokiTakingResponse(VokiId, SessionId, StartedAt, TotalQuestionsCount)
    {
        public override bool NewSessionStarted => false;

        public static StartVokiTakingActiveSessionExistsResponse Create(StartVokiTakingCommandActiveSessionExistsResult res) =>
            new(
                res.VokiId.ToString(),
                res.SessionId.ToString(),
                res.StartedAt,
                res.QuestionsWithSavedAnswersCount,
                res.TotalQuestionsCount
            );
    }
}