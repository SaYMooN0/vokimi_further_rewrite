using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record SetVokiCoverToDefaultCommand(VokiId VokiId) :
    ICommand<VokiCoverKey>,
    IWithVokiAccessValidationStep;

internal sealed class SetVokiCoverToDefaultCommandHandler :
    ICommandHandler<SetVokiCoverToDefaultCommand, VokiCoverKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public SetVokiCoverToDefaultCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiCoverKey>> Handle(SetVokiCoverToDefaultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        voki.SetCoverToDefault();
        await _draftGeneralVokiRepository.Update(voki);
        return voki.Cover;
    }
}