namespace VokimiStorageKeysLib.voki_cover;

internal static class VokiCoverKeyScheme
{
    public const string Template = "vokis/{vokiId:id}/cover";
    public static readonly ImmutableHashSet<string> AllowedExtensions = ["jpg", "webp"];
    private static readonly KeyTemplateParser Parser = new(Template, BaseStorageKey.Extensions.ImageFiles);

    public static ErrOrNothing IsKeyValid(string key, out VokiId vokiId) {
        var parseResult = Parser.TryParse(key);
        if (parseResult.IsErr(out var err)) {
            vokiId = default;
            return err;
        }

        var parts = parseResult.AsSuccess();
        vokiId = new VokiId(new Guid(parts["vokiId"]));
        return ErrOrNothing.Nothing;
    }
}