using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record CancelCoAuthorInviteCommand(VokiId VokiId, AppUserId NewCoAuthorId) :
    ICommand<DraftVoki>,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class CancelCoAuthorInviteCommandHandler :
    ICommandHandler<CancelCoAuthorInviteCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public CancelCoAuthorInviteCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<DraftVoki>> Handle(CancelCoAuthorInviteCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId))!;
        voki.CancelCoAuthorInvite(command.NewCoAuthorId);
        await _draftVokiRepository.Update(voki);
        return voki;
    }
}