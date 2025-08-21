using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys;

public class VokiCoverKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public override ImageFileExtension ImageExtension { get; }

    public VokiCoverKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var vokiId, out var ext)
        );

        VokiId = vokiId;
        ImageExtension = ext;
        Value = value;
    }


    public static ErrOr<VokiCoverKey> CreateWithId(VokiId id, ImageFileExtension extension) {
        var key = $"{KeyConsts.VokisFolder}/{id}/cover.{extension}";

        var validate = Scheme.IsKeyValid(key, out _, out _);
        if (validate.IsErr(out var err)) {
            return err;
        }

        return new VokiCoverKey(key);
    }

    public bool IsWithId(VokiId expectedId) =>  VokiId == expectedId;

    private static class Scheme
    {
        private const string Template = $"{KeyConsts.VokisFolder}/<vokiId:id>/cover.<ext:imageExt>";
        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

        public static ErrOrNothing IsKeyValid(string key, out VokiId vokiId, out ImageFileExtension ext) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                vokiId = default;
                ext = default;
                return err;
            }

            Dictionary<string, string> parts = parseResult.AsSuccess();

            vokiId = new VokiId(new Guid(parts["vokiId"]));
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();
           
            return ErrOrNothing.Nothing;
        }
    }
}