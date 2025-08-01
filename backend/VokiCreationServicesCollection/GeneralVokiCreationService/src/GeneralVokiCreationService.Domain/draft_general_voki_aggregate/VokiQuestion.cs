using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
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

    public bool ShuffleAnswers { get; private set; }
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
        ShuffleAnswers = false;
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


    public void UpdateText(VokiQuestionText questionText) {
        Text = questionText;
    }

    public ErrOrNothing UpdateImages(VokiQuestionImagesSet images) {
        if (images.Keys.Any(k => k.QuestionId != Id)) {
            return ErrFactory.Conflict("One or more images does not belong to this question");
        }

        Images = images;
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateAnswerSettings(QuestionAnswersCountLimit newCountLimit, bool shuffleAnswers) {
        if (newCountLimit.MaxAnswers != 1 && newCountLimit.MaxAnswers > _answers.Count) {
            return ErrFactory.Conflict("Max answers count limit cannot be greater than the number of answers");
        }

        AnswersCountLimit = newCountLimit;
        ShuffleAnswers = shuffleAnswers;
        return ErrOrNothing.Nothing;
    }

    public ErrOr<VokiQuestionAnswer> AddNewAnswer(
        BaseVokiAnswerTypeData answerData,
        ImmutableHashSet<GeneralVokiResultId> relatedResultIds
    ) {
        if (_answers.Count >= MaxAnswersCount) {
            return ErrFactory.LimitExceeded(
                "Answer count limit exceeded",
                $"Maximum allowed answers count is {MaxAnswersCount}. Current answers count is {_answers.Count}"
            );
        }

        if (answerData.MatchingEnum != this.AnswersType) {
            return ErrFactory.Conflict(
                "Given answer type does not correspond with the question answers type",
                $"Answers data type: {answerData.MatchingEnum}. Question answers type: {this.AnswersType}"
            );
        }

        var creationRes = VokiQuestionAnswer.CreateNew(
            answerData,
            (ushort)_answers.Count,
            relatedResultIds
        );
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        var answer = creationRes.AsSuccess();
        _answers.Add(answer);
        return answer;
    }
}