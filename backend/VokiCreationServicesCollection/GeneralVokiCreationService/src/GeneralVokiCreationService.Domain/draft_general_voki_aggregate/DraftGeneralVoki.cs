using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using SharedKernel;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;
using VokimiStorageKeysLib.general_voki.result_image;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public sealed class DraftGeneralVoki : BaseDraftVoki
{
    private const int
        MinQuestionsCount = 2,
        MaxQuestionsCount = 100;

    private const int
        MinResultsCount = 2,
        MaxResultsCount = 60;

    private DraftGeneralVoki() { }
    public VokiTakingProcessSettings TakingProcessSettings { get; private set; }
    private readonly List<VokiQuestion> _questions;
    public ImmutableArray<VokiQuestion> Questions => _questions.ToImmutableArray();
    private readonly List<VokiResult> _results;
    public ImmutableArray<VokiResult> Results => _results.ToImmutableArray();

    private DraftGeneralVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverKey cover,
        DateTime creationDate
    ) : base(
        vokiId, primaryAuthorId,
        name, cover,
        creationDate
    ) {
        TakingProcessSettings = VokiTakingProcessSettings.Default;
        _questions = [];
        _results = [];
    }

    public static DraftGeneralVoki Create(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverKey cover,
        DateTime creationDate
    ) {
        DraftGeneralVoki newGeneralVoki = new(vokiId, primaryAuthorId, name, cover, creationDate);
        newGeneralVoki.AddDomainEvent(
            new NewDraftVokiInitializedEvent(newGeneralVoki.Id, newGeneralVoki.PrimaryAuthorId));
        return newGeneralVoki;
    }

    public ErrOr<GeneralVokiQuestionId> AddNewQuestion(GeneralVokiAnswerType answersType) {
        if (_questions.Count >= MaxQuestionsCount) {
            return ErrFactory.LimitExceeded(
                $"General voki cannot have more than {MaxQuestionsCount} questions. Current count: {_questions.Count}"
            );
        }

        VokiQuestion question = VokiQuestion.CreateNew((ushort)_questions.Count, answersType);
        _questions.Add(question);
        return question.Id;
    }

    public void UpdateTakingProcessSettings(VokiTakingProcessSettings newSettings) {
        TakingProcessSettings = newSettings;
    }

    public ErrOr<VokiQuestion> QuestionWithId(GeneralVokiQuestionId questionId) {
        VokiQuestion? requestedQuestion = _questions.FirstOrDefault(q => q.Id == questionId);
        if (requestedQuestion is null) {
            return ErrFactory.NotFound.Common(
                "This voki doesn't have requested question",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        return requestedQuestion;
    }

    public ErrOr<VokiQuestion> UpdateQuestionText(GeneralVokiQuestionId questionId, VokiQuestionText newText) {
        VokiQuestion? questionToUpdate = _questions.FirstOrDefault(q => q.Id == questionId);
        if (questionToUpdate is null) {
            return ErrFactory.NotFound.Common(
                "Could not find question to update",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        questionToUpdate.UpdateText(newText);
        return questionToUpdate;
    }

    public ErrOr<VokiQuestion> UpdateQuestionImages(GeneralVokiQuestionId questionId, VokiQuestionImagesSet newImages) {
        VokiQuestion? questionToUpdate = _questions.FirstOrDefault(q => q.Id == questionId);
        if (questionToUpdate is null) {
            return ErrFactory.NotFound.Common(
                "Could not find question to update",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        var incorrectKeys = newImages.Keys
            .Where(k => !k.IsWithIds(Id, questionId))
            .Select(k => k.ToString())
            .ToArray();
        if (incorrectKeys.Length != 0) {
            return ErrFactory.Conflict(
                "Some of provided images does not belong to specified question",
                $"Voki id: {Id}, question id: {questionId}, incorrect keys: {string.Join(", ", incorrectKeys)}"
            );
        }

        var oldImages = questionToUpdate.Images;

        var res = questionToUpdate.UpdateImages(newImages);
        if (res.IsErr(out var err)) {
            return err;
        }

        AddDomainEvent(new VokiQuestionImagesUpdatedEvent(Id, questionId,
            OldImage: oldImages,
            NewImages: questionToUpdate.Images
        ));
        return questionToUpdate;
    }

    public ErrOr<VokiQuestion> UpdateQuestionAnswerSettings(
        GeneralVokiQuestionId questionId,
        QuestionAnswersCountLimit newCountLimit,
        bool shuffleAnswers
    ) {
        VokiQuestion? questionToUpdate = _questions.FirstOrDefault(q => q.Id == questionId);
        if (questionToUpdate is null) {
            return ErrFactory.NotFound.Common(
                "Could not find question to update",
                $"Voki with id {Id} doesn't have a question with id {questionId}"
            );
        }

        var res = questionToUpdate.UpdateAnswerSettings(newCountLimit, shuffleAnswers);
        if (res.IsErr(out var err)) {
            return err;
        }

        return questionToUpdate;
    }


    public ErrOr<VokiResult> ResultWithId(GeneralVokiResultId resultId) {
        VokiResult? requestedResult = _results.FirstOrDefault(q => q.Id == resultId);
        if (requestedResult is null) {
            return ErrFactory.NotFound.Common(
                "This voki doesn't have requested result",
                $"Voki with id {Id} doesn't have a result with id {resultId}"
            );
        }

        return requestedResult;
    }

    public ErrOrNothing AddNewResult(VokiResultName resultName, IDateTimeProvider dateTimeProvider) {
        if (_results.Count >= MaxResultsCount) {
            return ErrFactory.LimitExceeded(
                $"General voki cannot have more than {MaxResultsCount} results. Current count: {_results.Count}"
            );
        }

        HashSet<VokiResultName> existingNames = _results.Select(r => r.Name).ToHashSet();
        if (existingNames.Contains(resultName)) {
            return ErrFactory.Conflict("Result with this name already exists in this Voki. Result name must be unique");
        }

        VokiResult result = VokiResult.CreateNew(resultName, dateTimeProvider.UtcNow);
        _results.Add(result);
        return ErrOrNothing.Nothing;
    }

    public ErrOr<VokiResult> UpdateResult(
        GeneralVokiResultId resultId,
        VokiResultName newName,
        VokiResultText newText,
        GeneralVokiResultImageKey? newImage
    ) {
        VokiResult? resultToUpdate = _results.FirstOrDefault(q => q.Id == resultId);
        if (resultToUpdate is null) {
            return ErrFactory.NotFound.Common(
                "Could not find result to update",
                $"Voki with id {Id} doesn't have a result with id {resultId}"
            );
        }

        if (newImage is not null && !newImage.IsWithIds(Id, resultId)) {
            return ErrFactory.Conflict(
                "Provided image does not belong to the specified result",
                $"Voki id: {Id}, result id: {resultId}, key: {newImage}"
            );
        }

        var oldImage = resultToUpdate.Image;
        var updatingRes = resultToUpdate.Update(newName, newText, newImage);
        if (updatingRes.IsErr(out var err)) {
            return err;
        }

        AddDomainEvent(new VokiResultImageUpdatedEvent(Id, resultId,
            OldImage: oldImage,
            NewImage: resultToUpdate.Image
        ));

        return resultToUpdate;
    }


    public bool DeleteResult(GeneralVokiResultId resultId) {
        VokiResult? result = _results.FirstOrDefault(q => q.Id == resultId);
        if (result is null) {
            return false;
        }

        foreach (var q in _questions) {
            q.RemoveRelatedResultInAnswers(resultId);
        }

        _results.Remove(result);
        return true;
    }

    private ErrOrNothing CheckIfAnswerDataBelongs(GeneralVokiQuestionId questionId, BaseVokiAnswerTypeData answerData) {
        if (answerData is IVokiAnswerTypeDataWithStorageKey keyWithCheckNeeded) {
            if (!keyWithCheckNeeded.IsForCorrectVokiQuestion(Id, questionId)) {
                return ErrFactory.Conflict("Answer data does not belong to this question");
            }
        }

        return ErrOrNothing.Nothing;
    }

    private ErrOrNothing CheckIfResultsExist(ImmutableHashSet<GeneralVokiResultId> relatedResultIds) {
        var existingResults = _results.Select(r => r.Id).ToHashSet();
        var incorrectRelatedResultIds = relatedResultIds
            .Where(r => !existingResults.Contains(r))
            .Select(id => id.ToString())
            .ToArray();
        if (incorrectRelatedResultIds.Length > 0) {
            return ErrFactory.Conflict(
                "Some of the provided results specified as related does not exist in this voki",
                $"Voki id: {Id}, incorrect result ids: {string.Join(", ", incorrectRelatedResultIds)}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOr<VokiQuestionAnswer> AddNewAnswerToQuestion(
        GeneralVokiQuestionId questionId,
        BaseVokiAnswerTypeData answerData,
        ImmutableHashSet<GeneralVokiResultId> relatedResultIds
    ) {
        VokiQuestion? question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return ErrFactory.NotFound.Common("Cannot add new answer to question because question not fount");
        }

        if (
            CheckIfAnswerDataBelongs(questionId, answerData).IsErr(out var err)
            || CheckIfResultsExist(relatedResultIds).IsErr(out err)
        ) {
            return err;
        }

        var res = question.AddNewAnswer(answerData, relatedResultIds);
        return res;
    }

    public ErrOr<VokiQuestionAnswer> UpdateQuestionAnswer(
        GeneralVokiQuestionId questionId, GeneralVokiAnswerId answerId,
        BaseVokiAnswerTypeData newAnswerTypeData, ImmutableHashSet<GeneralVokiResultId> newRelatedResultIds
    ) {
        VokiQuestion? question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return ErrFactory.NotFound.Common("Cannot add update question answer because question doesn't exist");
        }

        if (
            CheckIfAnswerDataBelongs(questionId, newAnswerTypeData).IsErr(out var err)
            || CheckIfResultsExist(newRelatedResultIds).IsErr(out err)
        ) {
            return err;
        }

        return question.UpdateAnswer(answerId, newAnswerTypeData, newRelatedResultIds);
    }

    public bool DeleteQuestionAnswer(GeneralVokiQuestionId questionId, GeneralVokiAnswerId answerId) {
        VokiQuestion? question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null) {
            return false;
        }

        bool wasDeleted = question.DeleteAnswer(answerId);
        return wasDeleted;
    }

    private List<VokiPublishingIssue> CheckQuestionsForPublishingIssues() {
        if (_questions.Count < MinQuestionsCount) {
            return [
                VokiPublishingIssue.Problem(
                    message: $"Too few questions ({_questions.Count}). Minimum required is {MinQuestionsCount}",
                    source: "Questions",
                    fixRecommendation: $"Add at least {MinQuestionsCount - _questions.Count} more question(s)"
                )
            ];
        }

        if (_questions.Count > MaxQuestionsCount) {
            return [
                VokiPublishingIssue.Problem(
                    message: $"Too many questions ({_questions.Count}). Maximum allowed is {MaxQuestionsCount}",
                    source: "Questions",
                    fixRecommendation: $"Remove {_questions.Count - MaxQuestionsCount} question(s) to meet the limit"
                )
            ];
        }

        return _questions.SelectMany(q => q.CheckForPublishingIssues()).ToList();
    }


    private List<VokiPublishingIssue> CheckResultsForPublishingIssues() {
        List<VokiPublishingIssue> issues = [];

        if (_results.Count < MinResultsCount) {
            issues.Add(
                VokiPublishingIssue.Problem(
                    message: $"Too few results ({_results.Count}). Minimum required is {MinResultsCount}.",
                    source: "Results",
                    fixRecommendation: $"Add at least {MinResultsCount - _results.Count} more result(s)."
                )
            );
        }

        if (_results.Count > MaxResultsCount) {
            issues.Add(
                VokiPublishingIssue.Problem(
                    message: $"Too many results ({_results.Count}). Maximum allowed is {MaxResultsCount}.",
                    source: "Results",
                    fixRecommendation: $"Remove {_results.Count - MaxResultsCount} result(s) to meet the limit."
                )
            );
        }

        int resultImages = _results.Count(r => r.Image is not null);
        if (resultImages > 0 && resultImages < _results.Count) {
            issues.Add(
                VokiPublishingIssue.Warning(
                    message: "Some results have images while others do not.",
                    source: "Results",
                    fixRecommendation:
                    "Consider adding images to all results or removing them from all to keep presentation consistent."
                )
            );
        }

        HashSet<GeneralVokiResultId> resultsAnswersLeadTo = _questions
            .SelectMany(q => q.Answers.SelectMany(a => a.RelatedResultIds))
            .ToHashSet();

        foreach (var result in _results) {
            if (!resultsAnswersLeadTo.Contains(result.Id)) {
                issues.Add(
                    VokiPublishingIssue.Problem(
                        message: $"Result \"{result.Name}\" is not linked to any answer.",
                        source: "Results",
                        fixRecommendation: "Make this result related to at least one answer or remove it."
                    )
                );
            }
        }
        return issues;
    }


    public ImmutableArray<VokiPublishingIssue> CheckForPublishingIssues() => [
        ..base.CheckCoverForPublishingIssues(),
        ..base.CheckTagsForPublishingIssues(),
        ..base.CheckDetailsForPublishingIssues(),
        ..CheckQuestionsForPublishingIssues(),
        ..CheckResultsForPublishingIssues()
    ];

    public ErrOrNothing PublishWithWarningsIgnored(IDateTimeProvider dateTimeProvider) {
        var issues = CheckForPublishingIssues();
        bool anyProblems = issues.Any(i => i.Type == PublishingIssueType.Problem);
        if (anyProblems) {
            return ErrFactory.Conflict("Cannot publish Voki because of an unresolved problem");
        }

        QuestionDomainEventDto ParseQuestionToDto(VokiQuestion q) {
            return new QuestionDomainEventDto(
                q.Id, q.Text, q.Images, q.AnswersType, q.OrderInVoki,
                q.Answers.Select(a =>
                    new AnswerDomainEventDto(a.Id, a.OrderInQuestion, a.TypeData, a.RelatedResultIds.ToArray())
                ).ToArray(),
                q.ShuffleAnswers, q.AnswersCountLimit
            );
        }

        AddDomainEvent(new GeneralVokiPublishedEvent(
            Id, PrimaryAuthorId, CoAuthors,
            Name, Cover, Details, Tags,
            InitializingDate: CreationDate,
            PublishingDate: dateTimeProvider.UtcNow,
            _questions.Select(ParseQuestionToDto).ToArray(),
            ForceSequentialAnswering: TakingProcessSettings.ForceSequentialAnswering,
            ShuffleQuestions: TakingProcessSettings.ShuffleQuestions,
            _results.Select(r => new ResultDomainEventDto(r.Id, r.Name, r.Text, r.Image)).ToArray()
        ));
        return ErrOrNothing.Nothing;
    }
}