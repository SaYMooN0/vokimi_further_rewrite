using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Domain.app_user_aggregate;

public record UserProfileSettings(
    UserBanner Banner,
    UserStatus Status,
    UserPronouns Pronouns,
    UserAboutMe AboutMe,
    UserLinksSetting Links
)
{
    public static UserProfileSettings Default() => new(
        new UserBanner.Default(),
        UserStatus.Default(),
        UserPronouns.Default(),
        UserAboutMe.Default(),
        UserLinksSetting.Default()
    );
}