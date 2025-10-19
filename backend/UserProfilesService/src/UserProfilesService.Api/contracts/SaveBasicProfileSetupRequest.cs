using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Api.contracts;

public class SaveBasicProfileSetupRequest : IRequestWithValidationNeeded
{
    public string ProfilePic { get; init; }
    public string UserDisplayName { get; init; }
    public Language[] PreferredLanguages { get; init; }
    public string[] Tags { get; init; }

    public ErrOrNothing Validate() {
        ErrOrNothing errs = ErrOrNothing.Nothing;
        if (
            !UserProfilePicKey.IsStringWithPicsPrefix(ProfilePic)
            && !ITempKey.IsStringWithTempPrefix(ProfilePic)
            && !PresetProfilePicKey.IsStringPresetKey(ProfilePic)
        ) {
            errs.AddNext(ErrFactory.IncorrectFormat(
                "Incorrect profile picture format",
                $"Provided path should be either {nameof(UserProfilePicKey)}, {nameof(ITempKey)} or {nameof(PresetProfilePicKey)}"
            ));
            errs.AddNextIfErr(Domain.app_user_aggregate.UserDisplayName.CheckForErr(UserDisplayName));
            string[] incorrectTags = Tags.Where(t => !VokiTagId.IsStringValidTag(t)).ToArray();
            if (incorrectTags.Length > 0) {
                errs.AddNext(ErrFactory.IncorrectFormat(
                    "Some of the selected tags are invalid",
                    $"Incorrect tags: {string.Join(", ", incorrectTags)})"
                ));
            }
        }

        return errs;
    }
}