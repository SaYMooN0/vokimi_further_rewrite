using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public class UserLinksSetting : ValueObject
{
    public const int MaxLinksCount = 15;

    private UserLinksSetting(bool showInProfile, ImmutableArray<UserLink> links) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(showInProfile, links));
        ShowInProfile = showInProfile;
        Links = links;
    }

    public bool ShowInProfile { get; }
    public ImmutableArray<UserLink> Links { get; }

    public static UserLinksSetting Default() => new(
        false,
        ImmutableArray<UserLink>.Empty
    );

    public static ErrOr<UserLinksSetting> Create(bool showInProfile, ImmutableArray<UserLink> links) =>
        CheckForErr(showInProfile, links).IsErr(out var err) ? err : new UserLinksSetting(showInProfile, links);

    public static ErrOrNothing CheckForErr(bool showInProfile, ImmutableArray<UserLink> links) {
        if (links.Length > MaxLinksCount) {
            return ErrFactory.LimitExceeded(
                $"Too many links selected. User cannot specify more than {MaxLinksCount} in their profile"
            );
        }

        var uniqueLinksCount = links.Select(l => l.Value).ToHashSet().Count;
        if (uniqueLinksCount != links.Length) {
            return ErrFactory.Conflict(
                $"Some of the provided links are the same. Unique links count: {uniqueLinksCount}. Provided links count: {links.Length}",
                "Please remove duplicate links"
            );
        }

        if (showInProfile && links.Length == 0) {
            return ErrFactory.Conflict("No links to show in profile. Please add at least one link");
        }

        return ErrOrNothing.Nothing;
    }


    public override IEnumerable<object> GetEqualityComponents() => [
        ShowInProfile,
        Links.Select(l => (l.Value, l.Type))
    ];
}

public class UserLink
{
    public string Value { get; }
    public UserLinkType Type { get; }

    private UserLink(string value, UserLinkType type) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value, type));
        Value = value;
        Type = type;
    }

    public static ErrOrNothing CheckForErr(string value, UserLinkType type) {
        if (string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Link cannot be empty");
        }

        bool startsWithHttp = value.StartsWith("https://") || value.StartsWith("http://");
        if (!startsWithHttp) {
            return ErrFactory.NoValue.Common("Incorrect link format. Link must start with https:// or http://");
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOr<UserLink> Create(string value, UserLinkType type) =>
        CheckForErr(value, type).IsErr(out var err) ? err : new UserLink(value, type);
}

public enum UserLinkType
{
    Website,
    Telegram,
    Other
}