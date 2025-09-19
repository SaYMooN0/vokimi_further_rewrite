using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiQuestion : Entity<GeneralVokiQuestionId>
{
    private VokiQuestion() { }
    public string Text { get; }
    public VokiQuestionImagesSet ImageSet { get; }
    public GeneralVokiAnswerType AnswersType { get; }
    public ushort OrderInVoki { get; }
    public bool ShuffleAnswers { get; }
    public QuestionAnswersCountLimit AnswersCountLimit { get; }
    public IReadOnlyCollection<VokiQuestionAnswer> Answers { get; }


    public VokiQuestion(
        GeneralVokiQuestionId id,
        string text,
        VokiQuestionImagesSet imageSet,
        GeneralVokiAnswerType answersType,
        ushort orderInVoki,
        ImmutableArray<VokiQuestionAnswer> answers,
        bool shuffleAnswers, QuestionAnswersCountLimit answersCountLimit) {
        Id = id;
        Text = text;
        ImageSet = imageSet;
        AnswersType = answersType;
        OrderInVoki = orderInVoki;
        Answers = answers;
        ShuffleAnswers = shuffleAnswers;
        AnswersCountLimit = answersCountLimit;
    }
}