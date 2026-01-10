using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiDetailsCommand(VokiId VokiId, VokiDetails NewDetails) :
    ICommand<VokiDetails>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiDetailsCommandHandler : ICommandHandler<UpdateVokiDetailsCommand, VokiDetails>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public UpdateVokiDetailsCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task<ErrOr<VokiDetails>> Handle(UpdateVokiDetailsCommand command, CancellationToken ct) {
        BaseDraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        voki.UpdateDetails(command.NewDetails);
        await _draftVokiRepository.Update(voki, ct);
        return voki.Details;
    }
}