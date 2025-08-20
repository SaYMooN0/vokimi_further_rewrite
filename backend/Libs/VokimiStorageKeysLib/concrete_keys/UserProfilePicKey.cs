using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys;

public class UserProfilePicKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public AppUserId UserId { get; }
    public override ImageFileExtension ImageExtension { get; }

    public UserProfilePicKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var userId, out var ext)
        );

        UserId = userId;
        ImageExtension = ext;
        Value = value;
    }

    public static ErrOr<UserProfilePicKey> CreateNewForUser(AppUserId userId, ImageFileExtension extension) {
        var key = $"{KeyConsts.UserProfilePicsFolder}/{userId}.{extension}";

        var validate = Scheme.IsKeyValid(key, out _, out _);
        if (validate.IsErr(out var err)) {
            return err;
        }

        return new UserProfilePicKey(key);
    }

    public static ErrOr<UserProfilePicKey> CreateNewForUser(AppUserId userId, string extension) {
        var extOrErr = ImageFileExtension.Create(extension);
        if (extOrErr.IsErr(out var err)) {
            return err;
        }

        return CreateNewForUser(userId, extOrErr.AsSuccess());
    }

    public bool IsForUser(AppUserId userId) {
        return UserId == userId;
    }

    private static class Scheme
    {
        private const string Template = $"{KeyConsts.UserProfilePicsFolder}/<userId:id>.<ext:imageExt>";
        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

        public static ErrOrNothing IsKeyValid(string key, out AppUserId userId, out ImageFileExtension ext) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                userId = default;
                ext = default;
                return err;
            }

            var parts = parseResult.AsSuccess();

            userId = new AppUserId(new Guid(parts["userId"]));
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();
            
            return ErrOrNothing.Nothing;
        }
    }
}