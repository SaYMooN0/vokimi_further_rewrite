using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class LanguagesHashSetConverter : ValueConverter<HashSet<Language>, string[]>
{
    public LanguagesHashSetConverter() : base(
        langs => langs.Select(l => l.ToString()).ToArray(),
        strings => strings
            .Select(s => (Language)Enum.Parse(typeof(Language), s, true))
            .ToHashSet()
    ) { }
}

internal sealed class LanguagesHashSetComparer : ValueComparer<HashSet<Language>>
{
    public LanguagesHashSetComparer() : base(
        (a, b) => ReferenceEquals(a, b)
                  || (a == null && b == null)
                  || (a != null && b != null && a.SequenceEqual(b)),
        v => v.Aggregate(0, (acc, x) => HashCode.Combine(acc, x.GetHashCode())),
        v => v.ToHashSet()
    ) { }
}