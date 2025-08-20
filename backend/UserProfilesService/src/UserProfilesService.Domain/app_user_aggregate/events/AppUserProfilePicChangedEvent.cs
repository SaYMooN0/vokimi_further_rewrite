using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Domain.app_user_aggregate.events;

public sealed record class AppUserProfilePicChangedEvent(
    AppUserId UserId,
    UserProfilePicKey OldKey,
    UserProfilePicKey NewKey
) : IDomainEvent;