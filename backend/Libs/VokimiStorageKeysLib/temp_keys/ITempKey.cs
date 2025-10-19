using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.temp_keys;

public interface ITempKey : IEquatable<ITempKey>
{
    public string Value { get; }
    public IFileExtension Extension { get; }
    public static bool IsStringWithTempPrefix(string? value) => 
        !string.IsNullOrWhiteSpace(value) 
        && value.StartsWith(KeyConsts.TempFolder);

}