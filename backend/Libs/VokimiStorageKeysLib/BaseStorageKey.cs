using SharedKernel.domain;

namespace VokimiStorageKeysLib;

public abstract class BaseStorageKey : ValueObject
{
    protected abstract string Value { get; }
    public sealed override IEnumerable<object> GetEqualityComponents() => [Value];
    public sealed override string ToString() => Value;

    internal static class Extensions
    {
        public static readonly ImmutableHashSet<string> ImageFiles = ["jpg", "webp"];
        public static readonly ImmutableHashSet<string> AudioFiles = ["mp3"];
    }
}