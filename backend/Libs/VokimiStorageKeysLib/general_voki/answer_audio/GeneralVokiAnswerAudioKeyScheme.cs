namespace VokimiStorageKeysLib.general_voki.answer_audio;

sealed class GeneralVokiAnswerAudioKeyScheme
{
    public const string Template = "vokis/{vokiId:id}/questions/{questionId:id}/answers/{name:id}";
    private static readonly KeyTemplateParser Parser = new(Template, BaseStorageKey.Extensions.AudioFiles);

    public static ErrOrNothing IsKeyValid(
        string key,
        out VokiId vokiId,
        out GeneralVokiQuestionId questionId
    ) {
        var parseResult = Parser.TryParse(key);
        if (parseResult.IsErr(out var err)) {
            vokiId = default!;
            questionId = default!;
            return err;
        }

        var parts = parseResult.AsSuccess();
        vokiId = new VokiId(new Guid(parts["vokiId"]));
        questionId = new GeneralVokiQuestionId(new Guid(parts["questionId"]));
        return ErrOrNothing.Nothing;
    }
}