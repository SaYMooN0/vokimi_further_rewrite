using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public sealed record ImageOnly(
        GeneralVokiAnswerImageKey Image
    ) : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.ImageOnly;

        public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Image.IsWithIds(vokiId, questionId);
    }
}