using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestion : Entity<GeneralVokiQuestionId>
{
    public const int
        MinAnswersCount = 2,
        MaxAnswersCount = 60;

    private VokiQuestion() { }
    public VokiQuestionText Text { get; private set; }
    public VokiQuestionImagesSet Images { get; private set; }
    public GeneralVokiAnswerType AnswersType { get; }
    public ushort OrderInVoki { get; private set; }
    private readonly List<VokiQuestionAnswer> _answers;
    public int AnswersCount => _answers.Count();
    public QuestionAnswersCountLimit AnswersCountLimit { get; private set; }

    public VokiQuestion(
        GeneralVokiQuestionId id,
        VokiQuestionText text,
        ushort orderInVoki,
        GeneralVokiAnswerType answersType) {
        Id = id;
        Text = text;
        Images = VokiQuestionImagesSet.Empty;
        AnswersType = answersType;
        OrderInVoki = orderInVoki;
        _answers = [];
        AnswersCountLimit = QuestionAnswersCountLimit.SingleChoice();
    }
}