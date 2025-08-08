using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateVokiTakingProcessSettingsCommand(VokiId VokiId, VokiTakingProcessSettings NewSettings) :
    ICommand<VokiTakingProcessSettings>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiTakingProcessSettingsCommandHandler :
    ICommandHandler<UpdateVokiTakingProcessSettingsCommand, VokiTakingProcessSettings>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateVokiTakingProcessSettingsCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiTakingProcessSettings>> Handle(
        UpdateVokiTakingProcessSettingsCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        voki.UpdateTakingProcessSettings(command.NewSettings);
        await _draftGeneralVokiRepository.Update(voki);
        return voki.TakingProcessSettings;
    }
}