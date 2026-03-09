namespace UserProfilesService.Domain.app_user_aggregate;

public sealed class UserAboutMe : ValueObject
{
    public bool ShowInProfile { get; }
    public string Value { get; }
    public const int MaxLength = 1000;

    private UserAboutMe(bool showInProfile, string value) {
        ShowInProfile = showInProfile;
        Value = value;
    }

    public static UserAboutMe Disabled() => new(false, string.Empty);
    public static ErrOrNothing CheckForErr(bool enabled, string? value) {
        if (!enabled) {
            return ErrOrNothing.Nothing;
        }

        if (string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Value cannot be empty when enabled");
        }

        if (value.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Cannot exceed {MaxLength} characters");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [ShowInProfile, Value];
    public override string ToString() => Value;
}