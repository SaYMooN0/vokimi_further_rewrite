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
    public UserBanner Banner { get; private set; }
    public ProfilePic ProfilePic { get; private set; }

    public UserStatus Status { get; private set; }
    public UserPronouns Pronouns { get; private set; }
    public UserAboutMe AboutMe { get; private set; }
    public UserLanguageSettings  LanguageSettings{ get; private set; }
    public  UserLinksList Links { get; private set; }

    public ImmutableHashSet<VokiTagId> FavoriteTags { get; private set; }
    public ImmutableHashSet<AppUserId> FavoriteAuthors { get; private set; }

    //not immutable because ef core doesn't want to work with it for some reason
    private const int MaxFavoriteTagsCount = 20;
    public ProfileSettings Settings { get; private set; }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, ProfilePic profilePic)
    {
        if (!profilePic.Key.IsForUser(userId))
        {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Conflict(
                $"Given profile pic key doesn't belong to this user. User id: {userId}, profile pic id: {profilePic.Key.UserId}"
            ));
        }

        Id = userId;
        UniqueName = uniqueName;
        DisplayName = UserDisplayName.FromUniqueName(uniqueName);
        ProfilePic = profilePic;
        Banner = UserBanner.DefaultBanner;
        Status = UserStatus.Disabled();
        Pronouns = UserPronouns.Disabled();
        AboutMe = UserAboutMe.Disabled();
        KnownLanguages = UserKnownLanguages.Disabled();
        Links = [];
        FavoriteTags = [];
        FavoriteAuthors = [];
        PreferredLanguages = [];
        Settings = ProfileSettings.Default;
    }

    public AppUser(AppUserId userId, UserUniqueName uniqueName, UserProfilePicKey profilePicKey)
        : this(userId, uniqueName, new ProfilePic(profilePicKey, ProfilePicShape.Circle))
    {
    }

    private ErrOrNothing CheckIfProfilePicIsForUser(ProfilePic profilePic) => profilePic.Key.IsForUser(Id)
        ? ErrOrNothing.Nothing
        : ErrFactory.Conflict(
            "Given profile pic key doesn't belong to this user",
            $" User id: {Id}, profile pic id: {profilePic.Key.UserId}"
        );

    public ErrOrNothing UpdateProfilePic(ProfilePic newProfilePic)
    {
        if (CheckIfProfilePicIsForUser(newProfilePic).IsErr(out var err))
        {
            return err;
        }

        if (newProfilePic != ProfilePic)
        {
            var oldPic = ProfilePic;
            ProfilePic = newProfilePic;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldPic: oldPic, NewPic: newProfilePic));
        }

        return ErrOrNothing.Nothing;
    }

    private ErrOrNothing CheckFavouriteTagsSetForCount(ISet<VokiTagId> tags) => tags.Count > MaxFavoriteTagsCount
        ? ErrFactory.LimitExceeded($"Too many favourite tags chosen. Maximum count: {MaxFavoriteTagsCount}")
        : ErrOrNothing.Nothing;

    public ErrOrNothing ProcessBasicSetup(
        ProfilePic profilePic,
        UserDisplayName displayName,
        HashSet<Language> preferredLanguages,
        ImmutableHashSet<VokiTagId> favoriteTags
    )
    {
        if (
            CheckFavouriteTagsSetForCount(favoriteTags)
            .WithNextIfErr(CheckIfProfilePicIsForUser(profilePic))
            .IsErr(out var err)
        )
        {
            return err;
        }

        this.DisplayName = displayName;
        this.PreferredLanguages = preferredLanguages;
        this.FavoriteTags = favoriteTags;

        if (profilePic != ProfilePic)
        {
            var oldPic = ProfilePic;
            ProfilePic = profilePic;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldPic: oldPic, NewPic: profilePic));
        }

        return ErrOrNothing.Nothing;
    }
}