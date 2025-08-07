namespace VokimiStorageKeysLib.published_general_voki.result_image;

internal static class PublishedGeneralVokiResultImageKeyScheme
{
    public const string Template = "published-vokis/{vokiId:id}/results/{resultId:id}";
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