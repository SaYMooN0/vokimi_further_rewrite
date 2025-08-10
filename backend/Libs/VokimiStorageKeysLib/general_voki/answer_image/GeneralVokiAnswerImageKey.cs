namespace VokimiStorageKeysLib.general_voki.answer_image;

public class GeneralVokiAnswerImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }

    public GeneralVokiAnswerImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiAnswerImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        Value = value;
    }

    public static ErrOr<GeneralVokiAnswerImageKey> Create(string value) =>
        GeneralVokiAnswerImageKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new GeneralVokiAnswerImageKey(value);

    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId) =>
        VokiId == expectedVokiId && QuestionId == expectedQuestionId;
}