using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class AudioAndText : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public GeneralVokiAnswerText Text { get; }
        public GeneralVokiAnswerAudioKey Audio { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioAndText;

        public AudioAndText(GeneralVokiAnswerText text, GeneralVokiAnswerAudioKey audio) {
            Text = text;
            Audio = audio;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Text, Audio];

        public bool IsForCorrectVokiQuestion(
            VokiId vokiId,
            GeneralVokiQuestionId questionId,
            GeneralVokiAnswerId answerId
        ) => Audio.IsWithIds(vokiId, questionId, answerId);
    }
}