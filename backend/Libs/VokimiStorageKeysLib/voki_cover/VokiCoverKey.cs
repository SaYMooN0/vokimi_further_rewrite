namespace VokimiStorageKeysLib.voki_cover;

public class VokiCoverKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    private const string DefaultKeyValue = "common/default-voki-cover.webp";

    public VokiCoverKey(string value) {
        if (value == DefaultKeyValue) {
            Value = value;
            VokiId = new VokiId(Guid.Empty);
            return;
        }

        InvalidConstructorArgumentException.ThrowIfErr(this,
            VokiCoverKeyScheme.IsKeyValid(value, out var vokiId)
        );

        VokiId = vokiId;
        Value = value;
    }

    public static VokiCoverKey Default => new(DefaultKeyValue);

    public static ErrOr<VokiCoverKey> CreateWithId(VokiId id, string extension) {
        var key = $"{StorageFolders.Vokis}/{id}/cover{extension}";
        if (VokiCoverKeyScheme.IsKeyValid(key, out _).IsErr(out var err)) {
            return err;
        }

        return new VokiCoverKey(key);
    }

    public bool IsDefault() => Value == DefaultKeyValue;
    public bool IsWithId(VokiId expectedId) => IsDefault() || VokiId == expectedId;
}