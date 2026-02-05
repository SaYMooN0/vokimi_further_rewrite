using ApplicationShared;
using GeneralVokiTakingService.Application.common.dtos;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record ContinueVokiTakingCommand(
    VokiId VokiId
) : ICommand<VokiTakingData>;

internal sealed class ContinueVokiTakingCommandHandler : ICommandHandler<ContinueVokiTakingCommand, VokiTakingData>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IBaseTakingSessionsRepository _baseTakingSessionsRepository;

    public ContinueVokiTakingCommandHandler(
        IUserCtxProvider userCtxProvider,
        IGeneralVokisRepository generalVokisRepository,
        IBaseTakingSessionsRepository baseTakingSessionsRepository
    ) {
        _userCtxProvider = userCtxProvider;
        _generalVokisRepository = generalVokisRepository;
        _baseTakingSessionsRepository = baseTakingSessionsRepository;
    }

    public async Task<ErrOr<VokiTakingData>> Handle(ContinueVokiTakingCommand command, CancellationToken ct) {
        var vokiTakerCtx = _userCtxProvider.Current;
        if (!vokiTakerCtx.IsAuthenticated(out var aUserCtx)) {
            return ErrFactory.AuthRequired("You must be authenticated to continue voki taking");
        }

        BaseVokiTakingSession? session = await _baseTakingSessionsRepository.GetForVokiAndUser(command.VokiId, aUserCtx, ct);
        if (session is null) {
            return ErrFactory.NotFound.Voki("Active session not found",
                "No active voki taking session found for this user and voki");
        }

        var voki = await _generalVokisRepository.GetWithQuestions(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki not found", $"Voki with id: {command.VokiId} does not exist");
        }

        return VokiTakingData.Create(voki, session);
    }
}