using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record InviteCoAuthorCommand(
    VokiId VokiId,
    ImmutableHashSet<AppUserId> UserIdsToInvite
) :
    ICommand<DraftVoki>,
    IWithAuthCheckStep,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class InviteCoAuthorCommandHandler :
    ICommandHandler<InviteCoAuthorCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public InviteCoAuthorCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<DraftVoki>> Handle(InviteCoAuthorCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId, ct))!;
        ErrOrNothing result = voki.InviteCoAuthors(command.UserIdsToInvite);
        if (result.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki;
    }
}