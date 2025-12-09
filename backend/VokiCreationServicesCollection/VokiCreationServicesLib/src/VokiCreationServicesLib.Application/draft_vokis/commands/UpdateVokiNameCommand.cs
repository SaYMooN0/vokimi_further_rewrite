using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.common.vokis;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiNameCommand(
    VokiId VokiId,
    VokiName NewVokiName
) : ICommand<VokiName>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiNameCommandHandler : ICommandHandler<UpdateVokiNameCommand, VokiName>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public UpdateVokiNameCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<VokiName>> Handle(UpdateVokiNameCommand command, CancellationToken ct) {
        BaseDraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId, ct))!;
        voki.UpdateName(command.NewVokiName);
        await _draftVokiRepository.Update(voki, ct);
        return voki.Name;
    }
}