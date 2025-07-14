using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record SetVokiCoverToDefaultCommand(VokiId VokiId) :
    ICommand<DraftVokiCoverKey>,
    IWithVokiAccessValidationStep;

internal sealed class SetVokiCoverToDefaultCommandHandler :
    ICommandHandler<SetVokiCoverToDefaultCommand, DraftVokiCoverKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public SetVokiCoverToDefaultCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<DraftVokiCoverKey>> Handle(SetVokiCoverToDefaultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        voki.SetCoverToDefault();
        await _draftGeneralVokiRepository.Update(voki);
        return voki.Cover;
    }
}