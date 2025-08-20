namespace VokimiStorageKeysLib.temp_keys;

public interface ITempKey: IEquatable<ITempKey>
{
    public string Value { get; }
}