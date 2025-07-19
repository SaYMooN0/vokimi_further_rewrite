using SharedKernel.common.vokis;
using SharedKernel.exceptions;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public sealed class DraftGeneralVoki : BaseDraftVoki
{
    private DraftGeneralVoki() { }


    public VokiTakingProcessSettings TakingProcessSettings { get; private set; }
    private readonly List<VokiQuestion> _questions;
    public ImmutableArray<VokiQuestion> Questions => _questions.ToImmutableArray();

    private DraftGeneralVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, DraftVokiCoverKey cover,
        DateTime creationDate
    ) : base(
        vokiId, primaryAuthorId,
        name, cover,
        creationDate
    ) {
        TakingProcessSettings = VokiTakingProcessSettings.Default;
        _questions = [];
    }

    public static DraftGeneralVoki Create(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, DraftVokiCoverKey cover,
        DateTime creationDate
    ) {
        DraftGeneralVoki newGeneralVoki = new(vokiId, primaryAuthorId, name, cover, creationDate);
        newGeneralVoki.AddDomainEvent(
            new NewDraftVokiInitializedEvent(newGeneralVoki.Id, newGeneralVoki.PrimaryAuthorId));
        return newGeneralVoki;
    }

    public GeneralVokiQuestionId AddNewQuestion(GeneralVokiAnswerType answersType) {
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
}