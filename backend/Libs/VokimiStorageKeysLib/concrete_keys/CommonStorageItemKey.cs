using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys;

public class CommonStorageItemKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public string Name { get; }
    public override ImageFileExtension ImageExtension { get; }

    public CommonStorageItemKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var name, out var ext)
        );

        Name = name;
        ImageExtension = ext;
        Value = value;
    }

    public static readonly CommonStorageItemKey DefaultVokiCover = new(KeyConsts.DefaultVokiCover);
    public static readonly CommonStorageItemKey DefaultProfilePic = new(KeyConsts.DefaultUserProfilePic);

    private static class Scheme
    {
        private const string Template = $"{KeyConsts.CommonFolder}/<name:str>.<ext:imageExt>";
        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

        public static ErrOrNothing IsKeyValid(string key, out string name, out ImageFileExtension ext) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                name = default!;
                ext = default;
                return err;
            }

            var parts = parseResult.AsSuccess();

            name = parts["name"];
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}