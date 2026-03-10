using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public sealed class UserPronouns : ValueObject
{
    public bool ShowInProfile { get; }
    public string Value { get; }
    public const int MaxLength = 18;

    private UserPronouns(bool showInProfile, string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(showInProfile, value));
        ShowInProfile = showInProfile;
        Value = value;
    }

    public static UserPronouns Default() => new(false, string.Empty);

    public static ErrOr<UserPronouns> Create(bool showInProfile, string value) =>
        CheckForErr(showInProfile, value).IsErr(out var err)
            ? err
            : new UserPronouns(showInProfile, value);

    public static ErrOrNothing CheckForErr(bool enabled, string value) {
        if (enabled && string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Pronouns cannot be empty when enabled");
        }

        if (value.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Pronouns cannot exceed {MaxLength} characters");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [ShowInProfile, Value];
    public override string ToString() => ShowInProfile ? Value : "(Hidden)";
}