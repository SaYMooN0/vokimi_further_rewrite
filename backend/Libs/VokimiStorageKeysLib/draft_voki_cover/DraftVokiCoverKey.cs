using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.exceptions;

namespace VokimiStorageKeysLib.draft_voki_cover;

public class DraftVokiCoverKey : BaseStorageKey
{
    public override string Value { get; }
    private readonly VokiId _vokiId;
    private const string DefaultKeyValue = "/common/default-voki-cover.webp";

    public DraftVokiCoverKey(string value) {
        if (value == DefaultKeyValue) {
            Value = value;
            _vokiId = new VokiId(Guid.Empty);
            return;
        }

        var validationResult = DraftVokiCoverKeyScheme.IsKeyValid(value, out _vokiId);
        InvalidConstructorArgumentException.ThrowIfErr(this, validationResult);

        Value = value;
    }

    public static DraftVokiCoverKey Default => new (DefaultKeyValue);
    public bool IsDefault() => Value == DefaultKeyValue;
    public bool IsWithId(VokiId expectedId) => IsDefault() || _vokiId == expectedId;
    public static ErrOrNothing IsKeyValid(string key) => DraftVokiCoverKeyScheme.IsKeyValid(key, out _);
}