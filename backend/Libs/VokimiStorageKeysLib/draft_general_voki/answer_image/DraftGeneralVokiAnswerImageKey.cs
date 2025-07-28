namespace VokimiStorageKeysLib.draft_general_voki.answer_image;

public class DraftGeneralVokiAnswerImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }

    private DraftGeneralVokiAnswerImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiAnswerImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        Value = value;
    }

    public static ErrOr<DraftGeneralVokiAnswerImageKey> Create(string value) =>
        DraftGeneralVokiAnswerImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new DraftGeneralVokiAnswerImageKey(value);

    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId) =>
        VokiId == expectedVokiId && QuestionId == expectedQuestionId;
}