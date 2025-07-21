using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record UpdateVokiTagsCommand(VokiId VokiId, VokiTagsSet NewTags) :
    ICommand<VokiTagsSet>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiTagsCommandHandler :
    ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateVokiTagsCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiTagsSet>> Handle(UpdateVokiTagsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        voki.UpdateTags(command.NewTags);
        await _draftGeneralVokiRepository.Update(voki);
        return voki.Tags;
    }
}