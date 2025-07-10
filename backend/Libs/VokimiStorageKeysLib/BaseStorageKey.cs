using SharedKernel.domain;

namespace VokimiStorageKeysLib;

public abstract class BaseStorageKey : ValueObject
{
    protected abstract string Value { get; }
    public sealed override IEnumerable<object> GetEqualityComponents() => [Value];
    public sealed override string ToString() => Value;
}