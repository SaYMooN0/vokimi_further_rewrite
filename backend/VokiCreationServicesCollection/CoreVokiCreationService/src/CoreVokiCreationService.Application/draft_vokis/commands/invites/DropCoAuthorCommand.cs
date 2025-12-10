using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record DropCoAuthorCommand(
    VokiId VokiId,
    AppUserId CoAuthorId
) :
    ICommand<DraftVoki>,
    IWithAuthCheckStep,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class DropCoAuthorCommandHandler : ICommandHandler<DropCoAuthorCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public DropCoAuthorCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<DraftVoki>> Handle(DropCoAuthorCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId, ct))!;
        voki.DropCoAuthor(command.CoAuthorId);

        await _draftVokiRepository.Update(voki, ct);
        return voki;
    }
}