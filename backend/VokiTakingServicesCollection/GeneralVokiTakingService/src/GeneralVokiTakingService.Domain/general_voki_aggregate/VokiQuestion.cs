using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiQuestion : Entity<GeneralVokiQuestionId>
{
    private VokiQuestion() { }
    public string Text { get; }
    public VokiQuestionImagesSet ImageSet { get; }
    public ushort OrderInVoki { get; }
    public GeneralVokiQuestionContent Content { get; }
    public bool ShuffleAnswers { get; }
    public QuestionAnswersCountLimit AnswersCountLimit { get; }


    public VokiQuestion(
        GeneralVokiQuestionId id,
        string text,
        VokiQuestionImagesSet imageSet,
        ushort orderInVoki,
        bool shuffleAnswers,
        QuestionAnswersCountLimit answersCountLimit,
        GeneralVokiQuestionContent content
    ) {
        Id = id;
        Text = text;
        ImageSet = imageSet;
        OrderInVoki = orderInVoki;
        ShuffleAnswers = shuffleAnswers;
        AnswersCountLimit = answersCountLimit;
        Content = content;
    }

    public string Preview() => string.IsNullOrEmpty(Text)
        ? ""
        : Text.Length < 30
            ? Text
            : Text.Substring(0, 25) + " ...";

    public string ChooseExpectedNumberOfAnswersText() {
        var min = AnswersCountLimit.MinAnswers;
        var max = AnswersCountLimit.MaxAnswers;
        return (min == max) ? $"Choose exactly {min} answer(s)" : $"Choose from {min} to {max} answers";
    }
}