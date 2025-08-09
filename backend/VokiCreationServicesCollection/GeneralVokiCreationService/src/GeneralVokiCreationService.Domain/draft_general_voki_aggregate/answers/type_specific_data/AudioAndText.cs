using VokimiStorageKeysLib.draft_general_voki.answer_audio;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;


public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class AudioAndText : BaseVokiAnswerTypeData, IVokiAnswerTypeDataWithStorageKey
    {
        public GeneralVokiAnswerText Text { get; }
        public DraftGeneralVokiAnswerAudioKey Audio { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioAndText;

        public AudioAndText(GeneralVokiAnswerText text, DraftGeneralVokiAnswerAudioKey audio) {
            Text = text;
            Audio = audio;
        }

        public override IEnumerable<object> GetEqualityComponents() => [Text, Audio];
        public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Audio.IsWithIds(vokiId, questionId);
    }
}