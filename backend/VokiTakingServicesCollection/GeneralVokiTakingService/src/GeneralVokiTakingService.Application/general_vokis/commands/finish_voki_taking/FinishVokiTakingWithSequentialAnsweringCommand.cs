namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public sealed record FinishVokiTakingWithSequentialAnsweringCommand(VokiId VokiId) :
    ICommand<FinishVokiTakingCommandResponse>;

internal sealed class FinishVokiTakingWithSequentialAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, FinishVokiTakingCommandResponse>
{
    public async Task<ErrOr<FinishVokiTakingCommandResponse>> Handle(
        FinishVokiTakingWithSequentialAnsweringCommand command,
        CancellationToken ct
    ) {
        return ErrFactory.NotImplemented();
    }
}