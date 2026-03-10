using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public sealed class UserAboutMe : ValueObject
{
    public bool ShowInProfile { get; }
    public string Value { get; }
    public const int MaxLength = 1000;

    private UserAboutMe(bool showInProfile, string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(showInProfile, value));
        ShowInProfile = showInProfile;
        Value = value;
    }

    public static UserAboutMe Default() => new(false, string.Empty);

    public static ErrOr<UserAboutMe> Create(bool showInProfile, string value) =>
        CheckForErr(showInProfile, value).IsErr(out var err) ? err : new UserAboutMe(showInProfile, value);

    public static ErrOrNothing CheckForErr(bool enabled, string value) {
        if (enabled && string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Value cannot be empty when enabled");
        }

        if (value.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Cannot exceed {MaxLength} characters");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [ShowInProfile, Value];
    public override string ToString() => ShowInProfile ? Value : "(Hidden)";
}