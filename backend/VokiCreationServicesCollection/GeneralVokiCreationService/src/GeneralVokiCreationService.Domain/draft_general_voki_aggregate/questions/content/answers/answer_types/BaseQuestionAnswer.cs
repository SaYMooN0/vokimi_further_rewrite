using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

public abstract partial record BaseQuestionAnswer(
    AnswerOrderInQuestion Order,
    ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
)
{
    public const int MaxRelatedResultsForAnswerCount = 30;
    public abstract GeneralVokiAnswerType MatchingEnum { get; }
  
}
public interface IVokiAnswerTypeDataWithStorageKey
{
    public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId);
}