 using UserProfilesService.Domain.app_user_aggregate.events;

namespace UserProfilesService.Application.app_users.domain_event_handlers;

public class AppUserProfilePicChangedEventHandler : IDomainEventHandler<AppUserProfilePicChangedEvent>
{
    public async Task Handle(AppUserProfilePicChangedEvent e, CancellationToken ct) {
        
        
    }
}