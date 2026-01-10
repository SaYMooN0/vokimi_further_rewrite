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
    private readonly IUserCtxProvider _userCtxProvider;

    public DeclineCoAuthorInviteCommandHandler(IAppUsersRepository appUsersRepository, IUserCtxProvider userCtxProvider) {
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOrNothing> Handle(DeclineCoAuthorInviteCommand command, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetByIdForUpdate(_userCtxProvider.AuthenticatedUserId, ct))!;
        user.DeclineCoAuthorInvite(command.VokiId);
        await _appUsersRepository.Update(user, ct);
        return ErrOrNothing.Nothing;
    }
}