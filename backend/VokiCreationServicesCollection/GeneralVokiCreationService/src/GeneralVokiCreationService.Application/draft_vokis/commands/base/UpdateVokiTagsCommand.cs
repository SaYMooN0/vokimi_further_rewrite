using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record UpdateVokiTagsCommand(VokiId VokiId, VokiTagsSet NewTags) :
    ICommand<VokiTagsSet>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiTagsCommandHandler :
    ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateVokiTagsCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiTagsSet>> Handle(UpdateVokiTagsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId))!;
        voki.UpdateTags(command.NewTags);
        await _draftGeneralVokisRepository.Update(voki);
        return voki.Tags;
    }
}