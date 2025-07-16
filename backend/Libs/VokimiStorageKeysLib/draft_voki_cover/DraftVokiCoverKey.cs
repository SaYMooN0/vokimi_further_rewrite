using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.exceptions;

namespace VokimiStorageKeysLib.draft_voki_cover;

public class DraftVokiCoverKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    private const string DefaultKeyValue = "/common/default-voki-cover.webp";

    public DraftVokiCoverKey(string value) {
        if (value == DefaultKeyValue) {
            Value = value;
            VokiId = new VokiId(Guid.Empty);
            return;
        }

        InvalidConstructorArgumentException.ThrowIfErr(this,
            DraftVokiCoverKeyScheme.IsKeyValid(value, out var vokiId)
        );
        
        VokiId = vokiId;
        Value = value;
    }

    public static DraftVokiCoverKey Default => new(DefaultKeyValue);

    public static ErrOr<DraftVokiCoverKey> CreateWithId(VokiId id, string extension) {
        var ket = $"/draft-vokis/{id}/cover.{extension}";
        if (DraftVokiCoverKeyScheme.IsKeyValid(ket, out _).IsErr(out var err)) {
            return err;
        }

        return new DraftVokiCoverKey(ket);
    }

    public bool IsDefault() => Value == DefaultKeyValue;
    public bool IsWithId(VokiId expectedId) => IsDefault() || VokiId == expectedId;
}