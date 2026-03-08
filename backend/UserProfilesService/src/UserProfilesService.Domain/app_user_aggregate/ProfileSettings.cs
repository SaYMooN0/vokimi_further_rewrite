using SharedKernel.common.app_users;

namespace UserProfilesService.Domain.app_user_aggregate;

public record class ProfileSettings(
    AllowCoAuthorInvitesSettingValue AllowCoAuthorInvites
)
{
    public static ProfileSettings Default => new ProfileSettings(
        AllowCoAuthorInvitesSettingValueExtensions.Default
    );
}