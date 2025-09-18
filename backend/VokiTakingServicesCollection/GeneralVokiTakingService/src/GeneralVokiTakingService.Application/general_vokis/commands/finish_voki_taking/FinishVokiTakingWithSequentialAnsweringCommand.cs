namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

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
    public async Task<ErrOr<GeneralVokiResultId>> Handle(FinishVokiTakingWithSequentialAnsweringCommand command, CancellationToken ct) {
        return ErrFactory.NotImplemented();
    }
}