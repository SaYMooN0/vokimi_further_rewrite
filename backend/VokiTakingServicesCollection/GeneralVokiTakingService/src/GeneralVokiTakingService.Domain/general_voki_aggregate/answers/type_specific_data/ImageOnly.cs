using VokimiStorageKeysLib;
using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.general_voki.answer_image;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ImageOnly : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public GeneralVokiAnswerImageKey Image { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageOnly;

        public ImageOnly(GeneralVokiAnswerImageKey image) {
            Image = image;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Image];
        public BaseStorageKey Key => Image;
    }
}