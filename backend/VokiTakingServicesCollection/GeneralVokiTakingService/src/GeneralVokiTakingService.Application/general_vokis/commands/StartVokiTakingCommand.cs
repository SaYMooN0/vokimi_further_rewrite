using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.auth;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record StartVokiTakingCommand(VokiId VokiId) :
    ICommand<StartVokiTakingCommandResponse>;

internal sealed class StartVokiTakingCommandHandler :
    ICommandHandler<StartVokiTakingCommand, StartVokiTakingCommandResponse>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;

    public StartVokiTakingCommandHandler(IGeneralVokisRepository generalVokisRepository, IUserContext userContext) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
    }


    public async Task<ErrOr<StartVokiTakingCommandResponse>> Handle(StartVokiTakingCommand command, CancellationToken ct) {
        var voki = await _generalVokisRepository.GetWithQuestionAnswersAsNoTracking(command.VokiId);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki was not found", $"Voki with id: {command.VokiId} does not exist"
            );
        }

        if (voki.ForceSequentialAnswering) {
            return await StartVokiTakingWithSequentialAnswering(voki);
        }
        else {
            return await StartVokiTakingWithFreeAnswering(voki);
        }
    }

    private async Task<StartVokiTakingWithSequentialAnsweringCommandResponse> StartVokiTakingWithSequentialAnswering(
        GeneralVoki voki
    ) { }

    private async Task<StartVokiTakingWithFreeAnsweringCommandResponse> StartVokiTakingWithFreeAnswering(
        GeneralVoki voki
    ) { }
}

public abstract record StartVokiTakingCommandResponse(VokiId Id, bool ForceSequentialAnswering);

public record StartVokiTakingWithSequentialAnsweringCommandResponse(VokiId Id)
    : StartVokiTakingCommandResponse(Id, true);

public record StartVokiTakingWithFreeAnsweringCommandResponse(VokiId Id)
    : StartVokiTakingCommandResponse(Id, false);