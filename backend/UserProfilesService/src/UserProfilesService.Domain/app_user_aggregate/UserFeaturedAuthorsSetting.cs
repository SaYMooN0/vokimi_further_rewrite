using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate;

public class UserFeaturedAuthorsSetting : ValueObject
{
    private const int MaxAuthorsCount = 5;

    public ImmutableHashSet<AppUserId> UserIds { get; }
    public bool ShowInProfile { get; }

    private UserFeaturedAuthorsSetting(ImmutableHashSet<AppUserId> userIds, bool showInProfile) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(userIds));
        UserIds = userIds;
        ShowInProfile = showInProfile;
    }

    public static UserFeaturedAuthorsSetting Default() => new([], false);

    public static ErrOrNothing CheckForErr(ISet<AppUserId> userIds) => userIds.Count > MaxAuthorsCount
        ? ErrFactory.LimitExceeded($"Too many featured authors chosen. Maximum count: {MaxAuthorsCount}")
        : ErrOrNothing.Nothing;

    public static ErrOr<UserFeaturedAuthorsSetting> Create(ImmutableHashSet<AppUserId> userIds, bool showInProfile) =>
        CheckForErr(userIds).IsErr(out var err) ? err : new UserFeaturedAuthorsSetting(userIds, showInProfile);

    public override IEnumerable<object> GetEqualityComponents() => [UserIds, ShowInProfile];
}