namespace VokimiStorageKeysLib.draft_general_voki.answer_audio;

public class DraftGeneralVokiAnswerAudioKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }

    private DraftGeneralVokiAnswerAudioKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiAnswerAudioKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        Value = value;
    }
    public static ErrOr<DraftGeneralVokiAnswerAudioKey> Create(string value) =>
        DraftGeneralVokiAnswerAudioKeyScheme.IsKeyValid(value, out _, out _).IsErr(out var err)
            ? err
            : new DraftGeneralVokiAnswerAudioKey(value);
    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId) =>
        VokiId == expectedVokiId && QuestionId == expectedQuestionId;
}