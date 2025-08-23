using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public sealed record ImageAndText(
        GeneralVokiAnswerText Text,
        GeneralVokiAnswerImageKey Image
    )
        : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageAndText;
        public BaseStorageKey Key => Image;
    }
}