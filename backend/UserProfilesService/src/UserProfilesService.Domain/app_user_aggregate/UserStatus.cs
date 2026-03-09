namespace UserProfilesService.Domain.app_user_aggregate;

public sealed class UserStatus : ValueObject
{
    public bool Enabled { get; }
    public string Value { get; }
    public const int MaxLength = 250;

    private UserStatus(bool enabled, string value)
    {
        Enabled = enabled;
        Value = value;
    }

    public static UserStatus Disabled() => new(false, string.Empty);

    public static ErrOr<UserStatus> EnabledWithValue(string value)
    {
        var error = CheckForErr(true, value);
        if (error.IsErr(out var err)) return err;
        return new UserStatus(true, value);
    }

    public static ErrOrNothing CheckForErr(bool enabled, string? value)
    {
        if (!enabled) { return ErrOrNothing.Nothing; }

        if (string.IsNullOrWhiteSpace(value))
        {
            return ErrFactory.NoValue.Common("Status cannot be empty when enabled");
        }

        if (value.Length > MaxLength)
        {
            return ErrFactory.IncorrectFormat($"Status cannot exceed {MaxLength} characters");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [Enabled, Value];
    public override string ToString() => Value;
}
