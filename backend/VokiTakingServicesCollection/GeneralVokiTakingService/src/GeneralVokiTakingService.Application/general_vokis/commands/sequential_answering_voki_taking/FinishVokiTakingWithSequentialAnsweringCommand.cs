namespace GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;

public sealed record FinishVokiTakingWithSequentialAnsweringCommand(
    VokiId VokiId
    // VokiTakingSessionId SessionId,
    // GeneralVokiQuestionId LastQuestionId,
    // HashSet<GeneralVokiAnswerId> ChosenAnswers,
    // ushort AnswerOrder,
    // DateTime ClientFinishTime
) : ICommand<GeneralVokiResultId>;

internal sealed class FinishVokiTakingWithSequentialAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, GeneralVokiResultId>
{
    public Task<ErrOr<GeneralVokiResultId>> Handle(FinishVokiTakingWithSequentialAnsweringCommand command, CancellationToken ct) {
        return Task.FromResult<ErrOr<GeneralVokiResultId>>(ErrFactory.NotImplemented());
    }
}