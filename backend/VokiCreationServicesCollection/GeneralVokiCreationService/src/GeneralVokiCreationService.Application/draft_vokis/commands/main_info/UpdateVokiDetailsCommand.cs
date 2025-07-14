using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.main_info;

public sealed record UpdateVokiDetailsCommand(VokiId VokiId, VokiDetails NewDetails) :
    ICommand<VokiDetails>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiDetailsCommandHandler :
    ICommandHandler<UpdateVokiDetailsCommand, VokiDetails>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateVokiDetailsCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiDetails>> Handle(UpdateVokiDetailsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        voki.UpdateDetails(command.NewDetails);
        await _draftGeneralVokiRepository.Update(voki);
        return voki.Details;
    }
}