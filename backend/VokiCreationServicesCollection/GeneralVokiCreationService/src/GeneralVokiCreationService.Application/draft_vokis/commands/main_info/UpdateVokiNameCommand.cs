using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.main_info;

public sealed record UpdateVokiNameCommand(VokiId VokiId, VokiName NewVokiName) :
    ICommand<VokiName>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiNameCommandHandler : ICommandHandler<UpdateVokiNameCommand, VokiName>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateVokiNameCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiName>> Handle(UpdateVokiNameCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        voki.UpdateName(command.NewVokiName);
        await _draftGeneralVokiRepository.Update(voki);
        return voki.Name;
    }
}