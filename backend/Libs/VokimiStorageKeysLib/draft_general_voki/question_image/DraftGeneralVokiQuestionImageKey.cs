namespace VokimiStorageKeysLib.draft_general_voki.question_image;

public class DraftGeneralVokiQuestionImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }

    public DraftGeneralVokiQuestionImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiQuestionImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        Value = value;
    }

    public static ErrOr<DraftGeneralVokiQuestionImageKey> FromString(string value) =>
        DraftGeneralVokiQuestionImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new DraftGeneralVokiQuestionImageKey(value);

    public static DraftGeneralVokiQuestionImageKey Create(
        VokiId vokiId, GeneralVokiQuestionId questionId, string extension
    ) => new($"{Folder(vokiId, questionId)}/{Guid.NewGuid()}{extension}");

    public static string Folder(VokiId vokiId, GeneralVokiQuestionId questionId) =>
        $"draft-vokis/{vokiId}/questions/{questionId}/images";

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiQuestionId expectedQuestionId
    ) => VokiId == expectedVokiId && QuestionId == expectedQuestionId;
}