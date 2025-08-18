using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.contracts;

public record class UserPreviewResponse(
    string Id,
    string Name,
    string ProfilePic
)
{
    public static UserPreviewResponse Create(AppUser user) => new(
        user.Id.ToString(),
        user.UserName.ToString(),
        user.ProfilePic.ToString()
    );
}