using System.Text.RegularExpressions;

namespace SharedKernel.common;

public class AppUserName : ValueObject
{
    private readonly string _value;

    public AppUserName(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    private const int MinLength = 2;
    private const int MaxLength = 30;

    private const string AllowedCharsPattern = @"^[a-zA-Zа-яА-ЯёЁ0-9!@""'\$\%\^&;\*\(\)\[\]\-=_~`]+$";

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];

    public static ErrOr<AppUserName> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new AppUserName(value);

    public static ErrOrNothing CheckForErr(string username) {
        if (string.IsNullOrWhiteSpace(username) || username.Length < MinLength || username.Length > MaxLength) {
            return ErrFactory.IncorrectFormat(
                $"Username must be between {MinLength} and {MaxLength} characters"
            );
        }

        if (!Regex.IsMatch(username, AllowedCharsPattern)) {
            var invalidChars = username
                .Where(c => !Regex.IsMatch(c.ToString(), AllowedCharsPattern))
                .Distinct()
                .Select(c => $"'{c}'");

            string details = $"Username contains invalid characters: {string.Join(", ", invalidChars)}";

            return ErrFactory.IncorrectFormat("Username contains invalid characters", details);
        }

        return ErrOrNothing.Nothing;
    }
}