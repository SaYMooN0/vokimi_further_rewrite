namespace VokimiStorageKeysLib.published_voki_cover;

public class PublishedVokiCoverKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    private const string DefaultKeyValue = "common/default-voki-cover.webp";

    public PublishedVokiCoverKey(string value) {
        if (value == DefaultKeyValue) {
            Value = value;
            VokiId = new VokiId(Guid.Empty);
            return;
        }

        InvalidConstructorArgumentException.ThrowIfErr(this,
            PublishedVokiCoverKeyScheme.IsKeyValid(value, out var vokiId)
        );

        VokiId = vokiId;
        Value = value;
    }

    public static PublishedVokiCoverKey Default => new(DefaultKeyValue);

    public static ErrOr<PublishedVokiCoverKey> CreateWithId(VokiId id, string extension) {
        var key = $"{StorageFolders.PublishedVokis}/{id}/cover{extension}";
        if (PublishedVokiCoverKeyScheme.IsKeyValid(key, out _).IsErr(out var err)) {
            return err;
        }

        return new PublishedVokiCoverKey(key);
    }

    public bool IsDefault() => Value == DefaultKeyValue;
    public bool IsWithId(VokiId expectedId) => IsDefault() || VokiId == expectedId;
}