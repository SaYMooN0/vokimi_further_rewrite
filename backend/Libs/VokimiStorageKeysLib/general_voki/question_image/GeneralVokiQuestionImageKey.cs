namespace VokimiStorageKeysLib.general_voki.question_image;

public class GeneralVokiQuestionImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }

    public GeneralVokiQuestionImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiQuestionImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        Value = value;
    }

    public static ErrOr<GeneralVokiQuestionImageKey> FromString(string value) =>
        GeneralVokiQuestionImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new GeneralVokiQuestionImageKey(value);

    public static GeneralVokiQuestionImageKey Create(
        VokiId vokiId, GeneralVokiQuestionId questionId, string extension
    ) => new($"{Folder(vokiId, questionId)}/{Guid.NewGuid()}{extension}");

    public static string Folder(VokiId vokiId, GeneralVokiQuestionId questionId) =>
        $"{StorageFolders.Vokis}/{vokiId}/questions/{questionId}/images";

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiQuestionId expectedQuestionId
    ) => VokiId == expectedVokiId && QuestionId == expectedQuestionId;
}