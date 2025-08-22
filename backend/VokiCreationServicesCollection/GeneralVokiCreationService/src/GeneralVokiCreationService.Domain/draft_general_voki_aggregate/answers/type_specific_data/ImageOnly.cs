using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

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

        public bool IsForCorrectVokiQuestion(
            VokiId vokiId,
            GeneralVokiQuestionId questionId,
            GeneralVokiAnswerId answerId
        ) => Image.IsWithIds(vokiId, questionId, answerId);
    }
}