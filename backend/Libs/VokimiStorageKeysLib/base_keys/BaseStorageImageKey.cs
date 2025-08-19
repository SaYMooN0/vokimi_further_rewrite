namespace VokimiStorageKeysLib;

public abstract class BaseStorageImageKey : BaseStorageKey
{
    public static readonly ImmutableHashSet<string> AllowedExtensions = ["jpg", "webp"];

}