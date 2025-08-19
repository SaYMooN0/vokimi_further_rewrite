using SharedKernel.common.app_users;
using SharedKernel.exceptions;
using UserProfilesService.Domain.app_user_aggregate.events;
using VokimiStorageKeysLib.users;

namespace UserProfilesService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }

    public AppUserName UserName { get; private set; }
    public UserProfilePicKey ProfilePic { get; private set; }
    public ImmutableHashSet<VokiTagId> FavouriteTags { get; private set; }
    public UserSettings Settings { get; private set; }

    public AppUser(AppUserId userId, AppUserName userName, UserProfilePicKey profilePic) {
        if (!profilePic.IsForUser(userId)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Conflict(
                $"Given profile pic key doesn't belong to this user. User id: {userId}, profile pic id: {profilePic.UserId}"
            ));
        }

        Id = userId;
        UserName = userName;
        ProfilePic = profilePic;
        FavouriteTags = [];
        Settings = UserSettings.Default;
    }

    public void UpdateUserName(AppUserName newUserName) {
        if (newUserName != UserName) {
            UserName = newUserName;
            AddDomainEvent(new AppUserNameChangedEvent(Id, newUserName));
        }
    }

    public ErrOrNothing UpdateProfilePic(UserProfilePicKey newProfilePic) {
        if (!newProfilePic.IsForUser(Id)) {
            return ErrFactory.Conflict(
                "Given profile pic key doesn't belong to this user",
                $" User id: {Id}, profile pic id: {newProfilePic.UserId}"
            );
        }

        if (newProfilePic != ProfilePic) {
            var oldPic = ProfilePic;
            ProfilePic = newProfilePic;
            AddDomainEvent(new AppUserProfilePicChangedEvent(Id, OldKey: oldPic, NewKey: newProfilePic));
        }

        return ErrOrNothing.Nothing;
    }
}