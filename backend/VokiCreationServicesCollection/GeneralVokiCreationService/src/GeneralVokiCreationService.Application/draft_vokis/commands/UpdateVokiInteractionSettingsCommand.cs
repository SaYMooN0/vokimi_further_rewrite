using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis.general_vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateVokiInteractionSettingsCommand(VokiId VokiId, GeneralVokiInteractionSettings NewSettings) :
    ICommand<GeneralVokiInteractionSettings>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiInteractionSettingsCommandHandler :
    ICommandHandler<UpdateVokiInteractionSettingsCommand, GeneralVokiInteractionSettings>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateVokiInteractionSettingsCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<GeneralVokiInteractionSettings>> Handle(
        UpdateVokiInteractionSettingsCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId, ct))!;
        voki.UpdateInteractionSettings(command.NewSettings);
        await _draftGeneralVokisRepository.Update(voki, ct);
        return voki.InteractionSettings;
    }
}