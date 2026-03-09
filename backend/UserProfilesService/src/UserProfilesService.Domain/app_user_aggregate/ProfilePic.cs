using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Domain.app_user_aggregate;

public record ProfilePic(
    UserProfilePicKey Key,
    ProfilePicShape Shape
);
public enum ProfilePicShape
{
    Circle,
    Squircle
}
