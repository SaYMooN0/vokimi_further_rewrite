using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.contracts;

public record UserBasicProfileSetupInfoResponse(
    string UserId,
    string UserUniqueName,
    string DisplayName,
    string[] FavouriteTags,
    string ProfilePicture,
    Language[] PreferredLanguages
) : ICreatableResponse<AppUser>
{
    public static ICreatableResponse<AppUser> Create(AppUser user) => new UserBasicProfileSetupInfoResponse(
        user.Id.ToString(),
        user.UniqueName.ToString(),
        user.DisplayName.ToString(),
        user.FavouriteTags.Select(t => t.ToString()).ToArray(),
        user.ProfilePic.ToString(),
        user.PreferredLanguages.ToArray()
    );
}