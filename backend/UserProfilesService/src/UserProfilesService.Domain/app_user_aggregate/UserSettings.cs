using SharedKernel.common.app_users;

namespace UserProfilesService.Domain.app_user_aggregate;

public record class UserSettings(
    AllowCoAuthorInvitesSettingValue AllowCoAuthorInvites
)
{
    public static UserSettings Default => new UserSettings(
        AllowCoAuthorInvitesSettingValueExtensions.Default
    );
}