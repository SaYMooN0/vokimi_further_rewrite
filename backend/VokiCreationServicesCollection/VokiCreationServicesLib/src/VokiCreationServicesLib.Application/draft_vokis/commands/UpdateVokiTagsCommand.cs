using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiTagsCommand(VokiId VokiId, VokiTagsSet NewTags) :
    ICommand<VokiTagsSet>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiTagsCommandHandler : ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public UpdateVokiTagsCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<VokiTagsSet>> Handle(UpdateVokiTagsCommand command, CancellationToken ct) {
        BaseDraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        voki.UpdateTags(command.NewTags);
        await _draftVokiRepository.Update(voki, ct);
        return voki.Tags;
    }
}