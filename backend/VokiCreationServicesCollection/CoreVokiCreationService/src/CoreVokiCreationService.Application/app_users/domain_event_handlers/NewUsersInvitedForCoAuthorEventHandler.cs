using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class NewUsersInvitedForCoAuthorEventHandler : IDomainEventHandler<NewUsersInvitedForCoAuthorEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewUsersInvitedForCoAuthorEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewUsersInvitedForCoAuthorEvent e, CancellationToken ct) {
        AppUser[] users = (await _appUsersRepository.ListWithIds(e.NewInvitedUserId, ct));
        if (users.Length == 0) {
            return;
        }

        foreach (var user in users) {
            ErrOrNothing res = user.InviteForCoAuthor(e.VokiId);
            UnexpectedBehaviourException.ThrowIfErr(res);
        }

        await _appUsersRepository.UpdateRange(users, ct);
    }
}