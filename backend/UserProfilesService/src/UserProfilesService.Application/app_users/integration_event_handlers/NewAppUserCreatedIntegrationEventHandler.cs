using MassTransit;
using SharedKernel.integration_events;
using UserProfilesService.Application.common;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Application.app_users.integration_event_handlers;

public class NewAppUserCreatedIntegrationEventHandler : IConsumer<NewAppUserCreatedIntegrationEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public NewAppUserCreatedIntegrationEventHandler(
        IAppUsersRepository appUsersRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _appUsersRepository = appUsersRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task Consume(ConsumeContext<NewAppUserCreatedIntegrationEvent> context) {
        var picRes = await _mainStorageBucket.CopyUserProfilePicFromDefaults(context.Message.CreatedUserId);
        if (picRes.IsErr(out var err)) {
           return;
        }

        AppUser user = new AppUser(
            context.Message.CreatedUserId,
            new(context.Message.UserName),
            picRes.AsSuccess()
        );
        await _appUsersRepository.Add(user);
    }
}