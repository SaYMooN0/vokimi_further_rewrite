using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.persistence.value_converters;

internal class GuidBasedIdHashSetConverter<T> : ValueConverter<HashSet<T>, string> where T : GuidBasedId
{
    public GuidBasedIdHashSetConverter()
        : base(
            ids => string.Join(',', ids.Select(id => id.ToString())),
            str => str.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => (T)Activator.CreateInstance(typeof(T), Guid.Parse(id)!))
                .ToHashSet()
        ) { }
}

internal class GuidBasedIdHashSetComparer<T> : ValueComparer<HashSet<T>> where T : GuidBasedId
{
    public GuidBasedIdHashSetComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t
    ) { }
}