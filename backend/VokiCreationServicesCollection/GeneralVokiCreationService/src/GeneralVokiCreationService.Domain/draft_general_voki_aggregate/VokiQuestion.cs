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
    public ImmutableArray<VokiQuestionAnswer> Answers => _answers.ToImmutableArray();

    public QuestionAnswersCountLimit AnswersCountLimit { get; private set; }

    private VokiQuestion(
        GeneralVokiQuestionId id,
        VokiQuestionText text,
        VokiQuestionImagesSet images,
        ushort orderInVoki,
        GeneralVokiAnswerType answersType,
        QuestionAnswersCountLimit answersCountLimit
    ) {
        Id = id;
        Text = text;
        Images = images;
        AnswersType = answersType;
        OrderInVoki = orderInVoki;
        _answers = [];
        AnswersCountLimit = answersCountLimit;
    }

    public static VokiQuestion CreateNew(
        ushort orderInVoki,
        GeneralVokiAnswerType answersType
    ) => new(
        GeneralVokiQuestionId.CreateNew(),
        GeneralVokiPresets.GetRandomQuestionText(),
        VokiQuestionImagesSet.Empty,
        orderInVoki,
        answersType,
        QuestionAnswersCountLimit.SingleChoice()
    );
}