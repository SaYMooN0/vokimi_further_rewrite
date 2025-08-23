using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public sealed record AudioOnly(
        GeneralVokiAnswerAudioKey Audio
    ) : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioOnly;
        public BaseStorageKey Key => Audio;
    }
}