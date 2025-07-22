namespace VokimiStorageKeysLib.draft_general_voki.answer_image;

public class DraftGeneralVokiAnswerImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }
    public GeneralVokiAnswerId AnswerId { get; }

    private DraftGeneralVokiAnswerImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiAnswerImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId, out var answerId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        AnswerId = answerId;
        Value = value;
    }

    public static ErrOr<DraftGeneralVokiAnswerImageKey> Create(string value) =>
        DraftGeneralVokiAnswerImageKeyScheme.IsKeyValid(value, out _, out _, out _).IsErr(out var err)
            ? err
            : new DraftGeneralVokiAnswerImageKey(value);

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiQuestionId expectedQuestionId,
        GeneralVokiAnswerId answerId
    ) => VokiId == expectedVokiId && QuestionId == expectedQuestionId && AnswerId == answerId;
}