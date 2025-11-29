using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateVokiTakingProcessSettingsCommand(VokiId VokiId, VokiTakingProcessSettings NewSettings) :
    ICommand<VokiTakingProcessSettings>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiTakingProcessSettingsCommandHandler :
    ICommandHandler<UpdateVokiTakingProcessSettingsCommand, VokiTakingProcessSettings>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateVokiTakingProcessSettingsCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiTakingProcessSettings>> Handle(
        UpdateVokiTakingProcessSettingsCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId))!;
        voki.UpdateTakingProcessSettings(command.NewSettings);
        await _draftGeneralVokisRepository.Update(voki);
        return voki.TakingProcessSettings;
    }
}