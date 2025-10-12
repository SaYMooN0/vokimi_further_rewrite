using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal class LanguagesImmutableHashSetConverter : ValueConverter<ImmutableHashSet<Language>, string[]>
{
    public LanguagesImmutableHashSetConverter() : base(
        langs => langs.Select(l => l.ToString()).ToArray(),
        strings => LangsFromStrings(strings)
    ) { }

    private static ImmutableHashSet<Language> LangsFromStrings(string[] strs) =>
        strs.Select(s =>
                Enum.TryParse(typeof(Language), s, ignoreCase: true, out var result)
                    ? (Language)result
                    : default
            )
            .ToImmutableHashSet();
}

internal class LanguagesImmutableHashSetComparer : ValueComparer<ImmutableHashSet<Language>>
{
    public LanguagesImmutableHashSetComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableHashSet()
    ) { }
}