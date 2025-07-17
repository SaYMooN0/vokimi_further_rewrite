using SharedKernel.exceptions;
using VokimiStorageKeysLib.draft_general_voki.answer_audio;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;


public abstract partial class BaseVokiAnswerTypeData
{
    public sealed class AudioAndText : BaseVokiAnswerTypeData
    {
        public string Text { get; }
        public DraftGeneralVokiAnswerAudioKey Audio { get; }
        public override GeneralVokiAnswerType MatchingEnum => GeneralVokiAnswerType.AudioAndText;

        private AudioAndText(string text, DraftGeneralVokiAnswerAudioKey audio) {
            InvalidConstructorArgumentException.ThrowIfErr(this, GeneralVokiAnswerRules.CheckAnswerTextForErrs(text));
            Text = text;
            Audio = audio;
        }

        public static ErrOr<AudioAndText> CreateNew(string text, DraftGeneralVokiAnswerAudioKey audio) =>
            GeneralVokiAnswerRules.CheckAnswerTextForErrs(text).IsErr(out var err)
                ? err
                : new AudioAndText(text, audio);

        public override IEnumerable<object> GetEqualityComponents() => [Text, Audio];
    }
}