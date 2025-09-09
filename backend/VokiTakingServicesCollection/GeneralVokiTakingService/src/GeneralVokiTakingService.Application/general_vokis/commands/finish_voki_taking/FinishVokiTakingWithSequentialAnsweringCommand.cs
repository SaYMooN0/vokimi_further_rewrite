using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public sealed record FinishVokiTakingWithSequentialAnsweringCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    Dictionary<GeneralVokiQuestionId, HashSet<GeneralVokiAnswerId>> ChosenAnswers
) : ICommand<VokiResult>;

internal sealed class FinishVokiTakingWithSequentialAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, VokiResult>
{
    public async Task<ErrOr<VokiResult>> Handle(FinishVokiTakingWithSequentialAnsweringCommand command, CancellationToken ct) {
        return ErrFactory.NotImplemented();
    }
}