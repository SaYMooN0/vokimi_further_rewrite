using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public sealed record AudioOnly(
        GeneralVokiAnswerAudioKey Audio
    ) : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioOnly;
        public bool IsForCorrectVokiQuestion(
            VokiId vokiId,
            GeneralVokiQuestionId questionId,
            GeneralVokiAnswerId answerId
        ) => Audio.IsWithIds(vokiId, questionId, answerId);

    }
}