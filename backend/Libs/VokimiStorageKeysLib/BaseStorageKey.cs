using SharedKernel.domain;

namespace VokimiStorageKeysLib;

public abstract class BaseStorageKey : ValueObject
{
    public abstract string Value { get; }
    public sealed override IEnumerable<object> GetEqualityComponents() => [Value];
}