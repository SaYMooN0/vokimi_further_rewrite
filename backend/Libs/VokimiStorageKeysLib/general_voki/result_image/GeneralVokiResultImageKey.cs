namespace VokimiStorageKeysLib.general_voki.result_image;

public class GeneralVokiResultImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiResultId ResultId { get; }

    public GeneralVokiResultImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiResultImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var resultId
        ));
        VokiId = vokiId;
        ResultId = resultId;
        Value = value;
    }

    public static ErrOr<GeneralVokiResultImageKey> FromString(string value) =>
        DraftGeneralVokiResultImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new GeneralVokiResultImageKey(value);

    public static GeneralVokiResultImageKey Create(
        VokiId vokiId, GeneralVokiResultId resultId, string extension
    ) => new($"{Folder(vokiId, resultId)}/{Guid.NewGuid()}{extension}");

    public static string Folder(VokiId vokiId, GeneralVokiResultId resultId) =>
        $"{KeyConsts.Vokis}/{vokiId}/results/{resultId}";

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiResultId expectedResultId
    ) => VokiId == expectedVokiId && ResultId == expectedResultId;
}