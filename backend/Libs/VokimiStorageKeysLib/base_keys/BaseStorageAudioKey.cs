namespace VokimiStorageKeysLib.base_keys;

public abstract class BaseStorageAudioKey: BaseStorageKey
{
    public static readonly ImmutableHashSet<string> AllowedExtensions = ["mp3"];

}