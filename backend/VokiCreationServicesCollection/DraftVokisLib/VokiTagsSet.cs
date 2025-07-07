using System.Collections.Immutable;

namespace DraftVokisLib;

public class VokiTagsSet : ValueObject
{
    // public ImmutableHashSet<VokiTagId> Tags { get; }
    public static VokiTagsSet Empty => new();

    public override IEnumerable<object> GetEqualityComponents() => throw new NotImplementedException();
}