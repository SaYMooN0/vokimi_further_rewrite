using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.auth;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record class AcceptCoAuthorInviteCommand(VokiId VokiId) :
    ICommand;

internal sealed class AcceptCoAuthorInviteCommandHandler :
    ICommandHandler<AcceptCoAuthorInviteCommand>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserContext _userContext;

    public AcceptCoAuthorInviteCommandHandler(IDraftVokiRepository draftVokiRepository, IUserContext userContext) {
        _draftVokiRepository = draftVokiRepository;
        _userContext = userContext;
    }


    public async Task<ErrOrNothing> Handle(AcceptCoAuthorInviteCommand command, CancellationToken ct) {
        AppUserId? userId = _userContext.AuthenticatedUserId;
        DraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId))!;
        ErrOrNothing result = voki.AcceptInviteBy(userId);
        if (result.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki);
        return ErrOrNothing.Nothing;
    }
}