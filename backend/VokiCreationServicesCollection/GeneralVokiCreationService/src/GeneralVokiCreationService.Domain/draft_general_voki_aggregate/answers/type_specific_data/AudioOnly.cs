using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class AudioOnly : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public GeneralVokiAnswerAudioKey Audio { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioOnly;

        public AudioOnly(GeneralVokiAnswerAudioKey audio) {
            Audio = audio;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Audio];

        public bool IsForCorrectVokiQuestion(
            VokiId vokiId,
            GeneralVokiQuestionId questionId,
            GeneralVokiAnswerId answerId
        ) => Audio.IsWithIds(vokiId, questionId, answerId);
    }
}