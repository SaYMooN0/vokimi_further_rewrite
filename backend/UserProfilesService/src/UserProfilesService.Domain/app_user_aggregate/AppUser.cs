using SharedKernel.common.app_users;
using SharedKernel.exceptions;
using UserProfilesService.Domain.app_user_aggregate.events;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }

    public UserUniqueName UniqueName { get; private set; }
    public UserDisplayName DisplayName { get; private set; }
    public ProfilePic ProfilePic { get; private set; }
    public UserFavoriteTagsSetting FavoriteTags { get; private set; }
    public UserFeaturedAuthorsSetting FeaturedAuthors { get; private set; }
    public UserFrontendSettings FrontendSettings { get; private set; }
    public UserLanguageSettings LanguageSettings { get; private set; }
    public UserProfileSettings ProfileSettings { get; private set; }
    public UserSocialInteractionSettings SocialInteractionSettings { get; private set; }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, ProfilePic profilePic) {
        if (!profilePic.Key.IsForUser(userId)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Conflict(
                $"Given profile pic key doesn't belong to this user. User id: {userId}, profile pic id: {profilePic.Key.UserId}"
            ));
        }

        Id = userId;
        UniqueName = uniqueName;
        DisplayName = UserDisplayName.FromUniqueName(uniqueName);
        ProfilePic = profilePic;

        FavoriteTags = UserFavoriteTagsSetting.Default();
        FeaturedAuthors = UserFeaturedAuthorsSetting.Default();
        FrontendSettings = UserFrontendSettings.Default();
        LanguageSettings = UserLanguageSettings.Default();
        ProfileSettings = UserProfileSettings.Default();
        SocialInteractionSettings = UserSocialInteractionSettings.Default;
    }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, UserProfilePicKey profilePicKey)
        : this(userId, uniqueName, new ProfilePic(profilePicKey, ProfilePicShape.Circle)) { }

    private ErrOrNothing CheckIfProfilePicIsForUser(ProfilePic profilePic) => profilePic.Key.IsForUser(Id)
        ? ErrOrNothing.Nothing
        : ErrFactory.Conflict(
            "Given profile pic key doesn't belong to this user",
            $" User id: {Id}, profile pic id: {profilePic.Key.UserId}"
        );

    public ErrOrNothing UpdateProfilePic(ProfilePic newProfilePic) {
        if (CheckIfProfilePicIsForUser(newProfilePic).IsErr(out var err)) {
            return err;
        }

        if (newProfilePic != ProfilePic) {
            var oldPic = ProfilePic;
            ProfilePic = newProfilePic;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldPic: oldPic, NewPic: newProfilePic));
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing ProcessBasicSetup(
        ProfilePic profilePic,
        UserDisplayName displayName,
        UserLanguageSettings languageSettings,
        UserFavoriteTagsSetting favoriteTags
    ) {
        if (
            CheckIfProfilePicIsForUser(profilePic)
            .IsErr(out var err)
        ) {
            return err;
        }

        DisplayName = displayName;
        LanguageSettings = languageSettings;
        FavoriteTags = favoriteTags;

        if (profilePic != ProfilePic) {
            var oldPic = ProfilePic;
            ProfilePic = profilePic;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldPic: oldPic, NewPic: profilePic));
        }

        return ErrOrNothing.Nothing;
    }
}