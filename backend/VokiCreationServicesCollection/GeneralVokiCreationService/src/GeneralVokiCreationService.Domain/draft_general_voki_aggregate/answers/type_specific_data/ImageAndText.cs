using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class ImageAndText : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public GeneralVokiAnswerText Text { get; }
        public GeneralVokiAnswerImageKey Image { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageAndText;

        public ImageAndText(GeneralVokiAnswerText text, GeneralVokiAnswerImageKey image) {
            Text = text;
            Image = image;
        }
        public override IEnumerable<object> GetEqualityComponents() => [Text, Image];

        public bool IsForCorrectVokiQuestion(
            VokiId vokiId,
            GeneralVokiQuestionId questionId,
            GeneralVokiAnswerId answerId
        ) => Image.IsWithIds(vokiId, questionId, answerId);
    }
}