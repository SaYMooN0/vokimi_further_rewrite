using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record UpdateVokiDetailsCommand(VokiId VokiId, VokiDetails NewDetails) :
    ICommand<VokiDetails>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiDetailsCommandHandler :
    ICommandHandler<UpdateVokiDetailsCommand, VokiDetails>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateVokiDetailsCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiDetails>> Handle(UpdateVokiDetailsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId))!;
        voki.UpdateDetails(command.NewDetails);
        await _draftGeneralVokisRepository.Update(voki);
        return voki.Details;
    }
}