using VokimiStorageKeysLib.draft_general_voki.answer_image;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ImageOnly : BaseVokiAnswerTypeData
    {
        public DraftGeneralVokiAnswerImageKey Image { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageOnly;

        public ImageOnly(DraftGeneralVokiAnswerImageKey image) {
            Image = image;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Image];
    }
}