namespace VokimiStorageKeysLib.concrete_keys;

public class VokiCoverKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }

    public VokiCoverKey(string value) {
        if (value == KeyConsts.DefaultVokiCover) {
            Value = value;
            VokiId = new VokiId(Guid.Empty);
            return;
        }

        InvalidConstructorArgumentException.ThrowIfErr(this, Scheme.IsKeyValid(value, out var vokiId));
        VokiId = vokiId;
        Value = value;
    }

    public static VokiCoverKey Default => new(KeyConsts.DefaultVokiCover);

    public static ErrOr<VokiCoverKey> CreateWithId(VokiId id, string extension) {
        var key = $"{KeyConsts.VokisFolder}/{id}/cover{extension}";
        if (Scheme.IsKeyValid(key, out _).IsErr(out var err)) {
            return err;
        }

        return new VokiCoverKey(key);
    }

    public bool IsDefault() => Value == KeyConsts.DefaultVokiCover;
    public bool IsWithId(VokiId expectedId) => IsDefault() || VokiId == expectedId;

    private static class Scheme
    {
        private const string Template = $"{KeyConsts.VokisFolder}/<vokiId:id>/cover";
        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

        public static ErrOrNothing IsKeyValid(string key, out VokiId vokiId) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                vokiId = default;
                return err;
            }

            Dictionary<string, string> parts = parseResult.AsSuccess();
            vokiId = new VokiId(new Guid(parts["vokiId"]));
            return ErrOrNothing.Nothing;
        }
    }
}