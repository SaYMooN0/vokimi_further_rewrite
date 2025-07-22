using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

public class CoAuthorInviteCreatedEventHandler : IDomainEventHandler<CoAuthorInviteCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    public CoAuthorInviteCreatedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(CoAuthorInviteCreatedEvent e, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetById(e.InvitedUserId))!;
        var res = user.InviteToCoAuthor(e.VokiId);
        UnexpectedBehaviourException.ThrowIfErr(res);
        await _appUsersRepository.Update(user);
    }
}