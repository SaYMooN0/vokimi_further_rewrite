using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.base_keys;

public abstract class BaseStorageAudioKey: BaseStorageKey
{
    public static readonly ImmutableHashSet<string> AllowedExtensions = AudioFileExtension.WhiteList;
    public abstract AudioFileExtension AudioExtension { get; }
    public sealed override IFileExtension Extension => AudioExtension;

}