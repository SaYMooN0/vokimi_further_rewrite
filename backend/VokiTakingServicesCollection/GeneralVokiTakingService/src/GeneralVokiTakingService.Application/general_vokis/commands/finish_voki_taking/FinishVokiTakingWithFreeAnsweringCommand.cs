namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public sealed record FinishVokiTakingWithFreeAnsweringCommand(VokiId VokiId) :
    ICommand<FinishVokiTakingCommandResponse>;

internal sealed class FinishVokiTakingWithFreeAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, FinishVokiTakingCommandResponse>
{
    public async Task<ErrOr<FinishVokiTakingCommandResponse>> Handle(
        FinishVokiTakingWithFreeAnsweringCommand command,
        CancellationToken ct
    ) {
        return ErrFactory.NotImplemented();
    }
}