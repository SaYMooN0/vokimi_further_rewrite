using SharedKernel.common.vokis;
using VokimiStorageKeysLib.draft_general_voki_answers.image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answer_type_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ImageOnly : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithKey
    {
        public DraftGeneralVokiAnswerImageKey Image { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageOnly;

        public ImageOnly(DraftGeneralVokiAnswerImageKey image) {
            Image = image;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Image];

        public bool IsForCorrectAnswer(
            VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId, GeneralVokiAnswerId answerId
        ) => Image.IsWithIds(expectedVokiId, expectedQuestionId, answerId);
    }
}