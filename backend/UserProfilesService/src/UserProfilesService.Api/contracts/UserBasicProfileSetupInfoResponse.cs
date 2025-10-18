using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.contracts;

public record UserBasicProfileSetupInfoResponse(
    string UserUniqueName,
    string DisplayName,
    Language[] PreferredLanguages,
    string[] FavoriteTags,
    string ProfilePicture,
    int MaxDisplayNameLength,
    int MaxTagLength
) : ICreatableResponse<AppUser>
{
    public static ICreatableResponse<AppUser> Create(AppUser user) => new UserBasicProfileSetupInfoResponse(
        user.UniqueName.ToString(),
        user.DisplayName.ToString(),
        user.PreferredLanguages.ToArray(),
        user.FavoriteTags.Select(t => t.ToString()).ToArray(),
        user.ProfilePic.ToString(),
        UserDisplayName.MaxLength,
        VokiTagId.MaxTagLength
    );
}