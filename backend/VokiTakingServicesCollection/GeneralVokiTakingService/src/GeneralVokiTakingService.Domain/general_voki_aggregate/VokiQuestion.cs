using GeneralVokiTakingService.Domain.general_voki_aggregate.answers;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiQuestion : Entity<GeneralVokiQuestionId>
{
    private VokiQuestion() { }
    public string Text { get; }
    public string[] Images { get; }
    public GeneralVokiAnswerType AnswersType { get; }
    public ushort OrderInVoki { get; }
    private ImmutableArray<BaseVokiQuestionAnswer> Answers { get; }

    public bool ShuffleAnswers { get; }
    public bool IsMultipleChoice => MinAnswers != 1 || MaxAnswers != 1;
    public ushort MinAnswers { get; }
    public ushort MaxAnswers { get; }

    public VokiQuestion(
        GeneralVokiQuestionId id,
        string text,
        string[] images,
        GeneralVokiAnswerType answersType,
        ushort orderInVoki,
        ImmutableArray<BaseVokiQuestionAnswer> answers,
        bool shuffleAnswers,
        ushort minAnswers,
        ushort maxAnswers
    ) {
        Id = id;
        Text = text;
        Images = images;
        AnswersType = answersType;
        OrderInVoki = orderInVoki;
        Answers = [];
        ShuffleAnswers = shuffleAnswers;
        MinAnswers = minAnswers;
        MaxAnswers = maxAnswers;
    }
}