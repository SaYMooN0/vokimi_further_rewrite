using SharedKernel.common.app_users;
using SharedKernel.exceptions;
using UserProfilesService.Domain.app_user_aggregate.events;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }

    public UserUniqueName UniqueName { get; private set; }
    public UserDisplayName DisplayName { get; private set; }
    public UserProfilePicKey ProfilePic { get; private set; }
    public ImmutableHashSet<VokiTagId> FavoriteTags { get; private set; }

    //not immutable because ef core doesn't want to work with it for some reason
    public HashSet<Language> PreferredLanguages { get; private set; }
    private const int MaxFavoriteTagsCount = 20;
    public UserSettings Settings { get; private set; }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, UserProfilePicKey profilePic) {
        if (!profilePic.IsForUser(userId)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Conflict(
                $"Given profile pic key doesn't belong to this user. User id: {userId}, profile pic id: {profilePic.UserId}"
            ));
        }

        Id = userId;
        UniqueName = uniqueName;
        DisplayName = UserDisplayName.Empty;
        ProfilePic = profilePic;
        FavoriteTags = [];
        PreferredLanguages = [];
        Settings = UserSettings.Default;
    }

    private ErrOrNothing CheckIfProfilePicIsForUser(UserProfilePicKey profilePic) => profilePic.IsForUser(Id)
        ? ErrOrNothing.Nothing
        : ErrFactory.Conflict(
            "Given profile pic key doesn't belong to this user",
            $" User id: {Id}, profile pic id: {profilePic.UserId}"
        );

    public ErrOrNothing UpdateProfilePic(UserProfilePicKey newProfilePic) {
        if (CheckIfProfilePicIsForUser(newProfilePic).IsErr(out var err)) {
            return err;
        }

        if (newProfilePic != ProfilePic) {
            var oldPic = ProfilePic;
            ProfilePic = newProfilePic;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldKey: oldPic, NewKey: newProfilePic));
        }

        return ErrOrNothing.Nothing;
    }

    private ErrOrNothing CheckFavouriteTagsSetForCount(ISet<VokiTagId> tags) => tags.Count > MaxFavoriteTagsCount
        ? ErrFactory.LimitExceeded($"Too many favourite tags chosen. Maximum count: {MaxFavoriteTagsCount}")
        : ErrOrNothing.Nothing;

    public ErrOrNothing ProcessBasicSetup(
        UserProfilePicKey profilePicKey,
        UserDisplayName displayName,
        HashSet<Language> preferredLanguages,
        ImmutableHashSet<VokiTagId> favoriteTags
    ) {
        if (
            CheckFavouriteTagsSetForCount(favoriteTags)
            .WithNextIfErr(CheckIfProfilePicIsForUser(profilePicKey))
            .IsErr(out var err)
        ) {
            return err;
        }

        this.DisplayName = displayName;
        this.PreferredLanguages = preferredLanguages;
        this.FavoriteTags = favoriteTags;

        if (profilePicKey != ProfilePic) {
            var oldPic = ProfilePic;
            ProfilePic = profilePicKey;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldKey: oldPic, NewKey: profilePicKey));
        }

        return ErrOrNothing.Nothing;
    }
}