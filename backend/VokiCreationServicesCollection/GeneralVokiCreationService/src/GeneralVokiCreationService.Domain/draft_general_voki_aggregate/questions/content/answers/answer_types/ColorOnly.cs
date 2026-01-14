using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

public abstract partial record BaseQuestionAnswer
{
    public sealed record ColorOnly(
        HexColor Color,
        AnswerOrderInQuestion Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Order, RelatedResultIds)
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.TextOnly;
    }
}
