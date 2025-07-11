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

        var validationResult = DraftVokiCoverKeyScheme.IsKeyValid(value, out var vokiId);
        VokiId = vokiId;
        InvalidConstructorArgumentException.ThrowIfErr(this, validationResult);

        Value = value;
    }

    public static DraftVokiCoverKey Default => new(DefaultKeyValue);

    public static DraftVokiCoverKey CreateWithId(VokiId id, string extension) =>
        new($"/draft-vokis/{id}/cover.{extension}");

    public bool IsDefault() => Value == DefaultKeyValue;
    public bool IsWithId(VokiId expectedId) => IsDefault() || VokiId == expectedId;
    public static ErrOrNothing IsKeyValid(string key) => DraftVokiCoverKeyScheme.IsKeyValid(key, out _);
}