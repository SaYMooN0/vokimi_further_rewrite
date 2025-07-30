namespace VokimiStorageKeysLib.draft_general_voki.result_image;

internal static class DraftGeneralVokiResultImageKeyScheme
{
    public const string Template = "draft-vokis/{vokiId:id}/results/{resultId:id}/images/{name:id}";
    public static readonly ImmutableHashSet<string> AllowedExtensions = ["jpg", "webp"];
    private static readonly KeyTemplateParser Parser = new(Template, BaseStorageKey.Extensions.ImageFiles);

    public static ErrOrNothing IsKeyValid(
        string key,
        out VokiId vokiId,
        out GeneralVokiResultId resultId
    ) {
        var parseResult = Parser.TryParse(key);
        if (parseResult.IsErr(out var err)) {
            vokiId = default!;
            resultId = default!;
            return err;
        }

        var parts = parseResult.AsSuccess();
        vokiId = new VokiId(new Guid(parts["vokiId"]));
        resultId = new GeneralVokiResultId(new Guid(parts["resultId"]));
        return ErrOrNothing.Nothing;
    }
}