using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

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
        if (newCountLimit.MaxAnswers > MaxAnswersCount) {
            return ErrFactory.LimitExceeded($"Maximum answers count cannot be greater than {MaxAnswersCount}");
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
            answerData, (ushort)_answers.Count, relatedResultIds
        );

        if (creationRes.IsErr(out var err)) {
            return err;
        }

        var answer = creationRes.AsSuccess();
        _answers.Add(answer);
        return answer;
    }

    public ErrOr<VokiQuestionAnswer> UpdateAnswer(
        GeneralVokiAnswerId answerId,
        BaseVokiAnswerTypeData newAnswerData,
        ImmutableHashSet<GeneralVokiResultId> newRelatedResultIds
    ) {
        if (newAnswerData.MatchingEnum != this.AnswersType) {
            return ErrFactory.Conflict(
                "Given answer type does not correspond with the question answers type",
                $"Answers data type: {newAnswerData.MatchingEnum}. Question answers type: {this.AnswersType}"
            );
        }

        VokiQuestionAnswer? answer = _answers.FirstOrDefault(a => a.Id == answerId);
        if (answer is null) {
            return ErrFactory.NotFound.Common("Cannot add update question answer because answer doesn't exist");
        }

        var updateRes = answer.Update(newAnswerData, newRelatedResultIds);
        if (updateRes.IsErr(out var err)) {
            return err;
        }

        return answer;
    }

    public bool DeleteAnswer(GeneralVokiAnswerId answerId) {
        VokiQuestionAnswer? answer = _answers.FirstOrDefault(a => a.Id == answerId);
        if (answer is null) {
            return false;
        }

        return _answers.Remove(answer);
    }

    public void RemoveRelatedResultInAnswers(GeneralVokiResultId resultId) {
        foreach (var a in _answers) {
            a.RemoveRelatedResult(resultId);
        }
    }

    public List<VokiPublishingIssue> CheckForPublishingIssues() {
        string questionText = Text.ToString();
        string preview = questionText.Length > 15
            ? questionText[..15] + "..."
            : questionText;

        if (_answers.Count < MinAnswersCount) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question has too few answers ({_answers.Count}). Minimum required is {MinAnswersCount}",
                    source: "Question answers",
                    fixRecommendation: $"Add at least {MinAnswersCount - _answers.Count} more answer(s)"
                )
            ];
        }

        if (_answers.Count > MaxAnswersCount) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question has too many answers ({_answers.Count}). Maximum allowed is {MaxAnswersCount}",
                    source: "Question answers",
                    fixRecommendation: $"Remove {_answers.Count - MaxAnswersCount} answer(s) to meet the limit"
                )
            ];
        }

        if (_answers.Count < AnswersCountLimit.MinAnswers) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question's answer count is below the configured minimum ({AnswersCountLimit.MinAnswers})",
                    source: "Question answers",
                    fixRecommendation:
                    $"Decrease the minimum limit or add at least {AnswersCountLimit.MinAnswers - _answers.Count} more answer(s)"
                )
            ];
        }

        if (_answers.Count < AnswersCountLimit.MaxAnswers) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question's answer count is below the configured minimum({AnswersCountLimit.MaxAnswers})",
                    source: "Question answers",
                    fixRecommendation:
                    $"Decrease the maximum limit or add at least {AnswersCountLimit.MaxAnswers - _answers.Count} answer(s)"
                )
            ];
        }

        return [];
    }
}