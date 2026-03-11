using SharedKernel.common.app_users;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Domain.app_user_aggregate.dtos;

public sealed record UserProfileViewDto(
    UserBanner Banner,
    UserDisplayName DisplayName,
    UserUniqueName UniqueName,
    UserProfilePicKey ProfilePicKey,
    UserProfilePossiblyHiddenField<string> Pronouns,
    UserProfilePossiblyHiddenField<string> Status,
    UserProfilePossiblyHiddenField<string> AboutMe,
    UserProfilePossiblyHiddenField<IReadOnlyList<UserLink>> Links,
    UserProfilePossiblyHiddenField<IReadOnlyList<string>> FavoriteTags,
    UserProfilePossiblyHiddenField<IReadOnlyList<AppUserId>> FavoriteAuthorIds
);

public sealed record UserProfilePossiblyHiddenField<T>(
    T Value,
    bool ShowOnProfile
);