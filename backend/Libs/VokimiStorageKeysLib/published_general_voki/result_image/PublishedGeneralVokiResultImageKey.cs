namespace VokimiStorageKeysLib.published_general_voki.result_image;

public class PublishedGeneralVokiResultImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiResultId ResultId { get; }

    public PublishedGeneralVokiResultImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            PublishedGeneralVokiResultImageKeyScheme.IsKeyValid(
                value, out var vokiId, out var resultId
            ));
        VokiId = vokiId;
        ResultId = resultId;
        Value = value;
    }

    public static ErrOr<PublishedGeneralVokiResultImageKey> FromString(string value) =>
        PublishedGeneralVokiResultImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new PublishedGeneralVokiResultImageKey(value);

    public static PublishedGeneralVokiResultImageKey Create(
        VokiId vokiId, GeneralVokiResultId resultId, string extension
    ) => new($"published-vokis/{vokiId}/results/{resultId}{extension}");

}