namespace VokimiStorageKeysLib.users;

public class UserProfilePicKeyScheme
{
    public const string Template = "user-profile-pics/{userId:id}/{version:Id}";
    private static readonly KeyTemplateParser Parser = new(Template, BaseStorageKey.Extensions.ImageFiles);

    public static ErrOrNothing IsKeyValid(string key, out AppUserId userId) {
        var parseResult = Parser.TryParse(key);
        if (parseResult.IsErr(out var err)) {
            userId = default;
            return err;
        }

        var parts = parseResult.AsSuccess();
        userId = new AppUserId(new Guid(parts["userId"]));
        return ErrOrNothing.Nothing;
    }
}