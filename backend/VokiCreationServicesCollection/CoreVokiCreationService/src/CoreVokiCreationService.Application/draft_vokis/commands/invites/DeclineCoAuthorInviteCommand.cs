using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record DeclineCoAuthorInviteCommand(VokiId VokiId) :
    ICommand,
    IWithAuthCheckStep;

internal sealed class DeclineCoAuthorInviteCommandHandler : ICommandHandler<DeclineCoAuthorInviteCommand>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public DeclineCoAuthorInviteCommandHandler(IAppUsersRepository appUsersRepository, IUserContext userContext) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
    }


    public async Task<ErrOrNothing> Handle(DeclineCoAuthorInviteCommand command, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetById(_userContext.AuthenticatedUserId, ct))!;
        user.DeclineCoAuthorInvite(command.VokiId);
        await _appUsersRepository.Update(user, ct);
        return ErrOrNothing.Nothing;
    }
}