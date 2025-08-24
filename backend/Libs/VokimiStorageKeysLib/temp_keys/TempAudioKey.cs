using SharedKernel.errs.utils;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.temp_keys;

public class TempAudioKey : ITempKey
{
    public string Value { get; }

    public AudioFileExtension Extension { get; }
    IFileExtension ITempKey.Extension => Extension;

    public TempAudioKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this, CheckAndExtractExtension(value, out var ext)
        );
        Value = value;
        Extension = ext;
    }

    public static TempAudioKey CreateWithExtenstion(AudioFileExtension ext) => new(
        $"{KeyConsts.TempFolder}/{Guid.NewGuid()}-{Guid.NewGuid()}.{ext}"
    );

    private ErrOrNothing CheckAndExtractExtension(string value, out AudioFileExtension ext) {
        ext = default;

        if (string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.IncorrectFormat("Key cannot be null or empty");
        }

        var parts = value.Split('/');
        if (parts.Length != 2) {
            return ErrFactory.IncorrectFormat("Key must be in format '{TempFolder}/filename.ext'");
        }

        if (!string.Equals(parts[0], KeyConsts.TempFolder, StringComparison.Ordinal)) {
            return ErrFactory.IncorrectFormat($"Key must start with '{KeyConsts.TempFolder}/'");
        }

        var fileName = parts[1];
        var dotIndex = fileName.LastIndexOf('.');
        if (dotIndex <= 0 || dotIndex == fileName.Length - 1) {
            return ErrFactory.IncorrectFormat("Key must contain a valid filename and extension");
        }

        var namePart = fileName[..dotIndex];
        var extPart = fileName[(dotIndex + 1)..];

        if (string.IsNullOrWhiteSpace(namePart) || namePart.Length > 150) {
            return ErrFactory.IncorrectFormat("Filename part must not be empty or longer than 150 characters");
        }

        var typedExt = AudioFileExtension.Create(extPart);
        if (typedExt.IsErr(out var err)) {
            return err;
        }

        ext = typedExt.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public bool Equals(ITempKey? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);
    public override string ToString() => Value;

}