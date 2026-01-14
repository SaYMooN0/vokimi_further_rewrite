using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;

public abstract partial record BaseQuestionAnswer
{
    public sealed record AudioAndText(
        GeneralVokiAnswerText Text,
        GeneralVokiAnswerAudioKey Audio,
        AnswerOrderInQuestion Order,
        ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
    ) : BaseQuestionAnswer(Order, RelatedResultIds), IVokiAnswerTypeDataWithStorageKey
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioAndText;

        public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Audio.IsWithIds(vokiId, questionId);
    }
}