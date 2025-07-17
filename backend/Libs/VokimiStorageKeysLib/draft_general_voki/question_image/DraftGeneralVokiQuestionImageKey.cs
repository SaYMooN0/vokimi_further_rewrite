namespace VokimiStorageKeysLib.draft_general_voki.question_image;

public class DraftGeneralVokiQuestionImageKey : BaseStorageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }
    public GeneralVokiAnswerId AnswerId { get; }

    public DraftGeneralVokiQuestionImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, DraftGeneralVokiQuestionImageKeyScheme.IsKeyValid(
            value, out var vokiId, out var questionId, out var answerId
        ));
        VokiId = vokiId;
        QuestionId = questionId;
        AnswerId = answerId;
        Value = value;
    }

    public bool IsWithIds(
        VokiId expectedVokiId,
        GeneralVokiQuestionId expectedQuestionId,
        GeneralVokiAnswerId answerId
    ) => VokiId == expectedVokiId && QuestionId == expectedQuestionId && AnswerId == answerId;
}