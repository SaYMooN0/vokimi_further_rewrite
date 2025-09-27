using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record UpdateVokiNameCommand(VokiId VokiId, VokiName NewVokiName) :
    ICommand<VokiName>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiNameCommandHandler : ICommandHandler<UpdateVokiNameCommand, VokiName>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateVokiNameCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiName>> Handle(UpdateVokiNameCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId))!;
        voki.UpdateName(command.NewVokiName);
        await _draftGeneralVokisRepository.Update(voki);
        return voki.Name;
    }
}