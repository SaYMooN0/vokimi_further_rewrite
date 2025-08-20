using VokimiStorageKeysLib;
using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.general_voki.answer_audio;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

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
        public BaseStorageKey Key => Audio;

    }
}