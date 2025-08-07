namespace VokimiStorageKeysLib.draft_general_voki.result_image;

public class DraftGeneralVokiResultImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiResultId ResultId { get; }

    public DraftGeneralVokiResultImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiResultImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var resultId
        ));
        VokiId = vokiId;
        ResultId = resultId;
        Value = value;
    }

    public static ErrOr<DraftGeneralVokiResultImageKey> FromString(string value) =>
        DraftGeneralVokiResultImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new DraftGeneralVokiResultImageKey(value);

    public static DraftGeneralVokiResultImageKey Create(
        VokiId vokiId, GeneralVokiResultId resultId, string extension
    ) => new($"{Folder(vokiId, resultId)}/{Guid.NewGuid()}{extension}");

    public static string Folder(VokiId vokiId, GeneralVokiResultId resultId) =>
        $"draft-vokis/{vokiId}/results/{resultId}";

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiResultId expectedResultId
    ) => VokiId == expectedVokiId && ResultId == expectedResultId;
}