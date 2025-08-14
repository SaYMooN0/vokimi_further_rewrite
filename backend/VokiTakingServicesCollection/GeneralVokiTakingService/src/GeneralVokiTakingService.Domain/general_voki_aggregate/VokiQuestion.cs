using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using VokimiStorageKeysLib.general_voki.question_image;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiQuestion : Entity<GeneralVokiQuestionId>
{
    private VokiQuestion() { }
    public string Text { get; }
    public GeneralVokiQuestionImageKey[] Images { get; }
    public GeneralVokiAnswerType AnswersType { get; }
    public ushort OrderInVoki { get; }
    public bool ShuffleAnswers { get; }
    public QuestionAnswersCountLimit AnswersCountLimit { get; }
    public ImmutableArray<VokiQuestionAnswer> Answers { get; }


    public VokiQuestion(
        GeneralVokiQuestionId id,
        string text,
        GeneralVokiQuestionImageKey[] images,
        GeneralVokiAnswerType answersType,
        ushort orderInVoki,
        ImmutableArray<VokiQuestionAnswer> answers,
        bool shuffleAnswers, QuestionAnswersCountLimit answersCountLimit) {
        Id = id;
        Text = text;
        Images = images;
        AnswersType = answersType;
        OrderInVoki = orderInVoki;
        Answers = answers;
        ShuffleAnswers = shuffleAnswers;
        AnswersCountLimit = answersCountLimit;
    }
}