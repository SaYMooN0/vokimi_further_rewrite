namespace VokimiStorageKeysLib.general_voki.question_image;

internal static class GeneralVokiQuestionImageKeyScheme
{
    public const string Template = "vokis/{vokiId:id}/questions/{questionId:id}/images/{specificImage:id}";
    public static readonly ImmutableHashSet<string> AllowedExtensions = ["jpg", "webp"];
    private static readonly KeyTemplateParser Parser = new(Template, BaseStorageKey.Extensions.ImageFiles);

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