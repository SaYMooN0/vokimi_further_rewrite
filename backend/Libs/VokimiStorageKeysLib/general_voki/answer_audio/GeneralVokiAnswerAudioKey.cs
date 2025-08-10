namespace VokimiStorageKeysLib.general_voki.answer_audio;

public class GeneralVokiAnswerAudioKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }

    private GeneralVokiAnswerAudioKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiAnswerAudioKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        Value = value;
    }
    public static ErrOr<GeneralVokiAnswerAudioKey> Create(string value) =>
        GeneralVokiAnswerAudioKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new GeneralVokiAnswerAudioKey(value);
    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId) =>
        VokiId == expectedVokiId && QuestionId == expectedQuestionId;
}