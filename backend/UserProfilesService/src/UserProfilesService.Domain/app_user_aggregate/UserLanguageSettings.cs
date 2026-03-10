using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate;

public sealed class UserLanguageSettings : ValueObject
{
    public bool ShowInProfile { get; }
    public ImmutableHashSet<Language> KnownLanguages { get; }
    public UnknownLanguagesSettings UnknownLanguages { get; }

    private UserLanguageSettings(
        ImmutableHashSet<Language> knownLanguages,
        bool showInProfile,
        UnknownLanguagesSettings unknownLanguages
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(knownLanguages, unknownLanguages));
        KnownLanguages = knownLanguages;
        ShowInProfile = showInProfile;
        UnknownLanguages = unknownLanguages;
    }

    public static UserLanguageSettings Default() => new(
        [],
        false,
        new UnknownLanguagesSettings(UnknownLanguagesSettingsValue.HideOnlyBlacklist, [])
    );

    public static ErrOr<UserLanguageSettings> Create(
        bool showOnProfile,
        ImmutableHashSet<Language> knownLanguages,
        UnknownLanguagesSettings unknownLanguages
    ) =>
        CheckForErr(knownLanguages, unknownLanguages).IsErr(out var err)
            ? err
            : new UserLanguageSettings(knownLanguages, showOnProfile, unknownLanguages);

    public static ErrOrNothing CheckForErr(
        ImmutableHashSet<Language> knownLanguages,
        UnknownLanguagesSettings unknownLanguages
    ) {
        if (knownLanguages.Intersect(unknownLanguages.Blacklist).Any()) {
            return ErrFactory.Conflict("Known languages and blacklisted languages cannot overlap");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [
        KnownLanguages,
        ShowInProfile,
        UnknownLanguages
    ];
}

public record UnknownLanguagesSettings(
    UnknownLanguagesSettingsValue Value,
    ImmutableHashSet<Language> Blacklist
);

public enum UnknownLanguagesSettingsValue
{
    HideAllUnknown,
    HideOnlyBlacklist
}