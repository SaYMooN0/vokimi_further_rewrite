using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.base_keys;

public abstract class BaseStorageImageKey : BaseStorageKey
{
    public static readonly ImmutableHashSet<string> AllowedExtensions = ImageFileExtension.WhiteList;
    public abstract ImageFileExtension ImageExtension { get; }
    public sealed override IFileExtension Extension => ImageExtension;
}
