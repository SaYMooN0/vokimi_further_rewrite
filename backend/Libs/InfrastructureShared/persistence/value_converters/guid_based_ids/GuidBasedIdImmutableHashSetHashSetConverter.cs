using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.persistence.value_converters.guid_based_ids;

internal class GuidBasedIdImmutableHashSetConverter<T> : ValueConverter<ImmutableHashSet<T>, Guid[]>
    where T : GuidBasedId
{
    public GuidBasedIdImmutableHashSetConverter()
        : base(
            ids => ids.Select(id => id.Value).ToArray(),
            guids => guids.Select(g => (T)Activator.CreateInstance(typeof(T), g)!)
                .ToImmutableHashSet()
        ) { }
}

internal class GuidBasedIdImmutableHashSetComparer<T> : ValueComparer<ImmutableHashSet<T>> where T : GuidBasedId
{
    public GuidBasedIdImmutableHashSetComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableHashSet()
    ) { }
}