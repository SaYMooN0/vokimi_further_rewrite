using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.contracts;

public record UserBasicProfileSetupInfoResponse(
    string UniqueName,
    string DisplayName,
    Language[] PreferredLanguages,
    string[] FavoriteTags,
    string ProfilePicKey,
    int MaxDisplayNameLength,
    int MaxTagLength
) : ICreatableResponse<AppUser>
{
    public static ICreatableResponse<AppUser> Create(AppUser user) => new UserBasicProfileSetupInfoResponse(
        user.UniqueName.ToString(),
        user.DisplayName.ToString(),
        user.LanguageSettings.KnownLanguages.ToArray(),
        user.FavoriteTagsSetting.Tags.Select(t => t.ToString()).ToArray(),
        user.ProfilePic.Key.ToString(),
        UserDisplayName.MaxLength,
        VokiTagId.MaxTagLength
    );
}