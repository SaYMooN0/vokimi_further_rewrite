using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Api.contracts;

public class UpdateProfilePictureRequest : IRequestWithValidationNeeded
{
    public string ProfilePic { get; init; }
    public ProfilePicShape Shape { get; init; }

    public ErrOrNothing Validate() {
        if (
            !UserProfilePicKey.IsStringWithPicsPrefix(ProfilePic)
            && !TempImageKey.IsPossiblySuitable(ProfilePic)
        ) {
            return ErrFactory.IncorrectFormat(
                "Incorrect profile picture format",
                $"Provided path should be either {nameof(UserProfilePicKey)} or {nameof(TempImageKey)}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}