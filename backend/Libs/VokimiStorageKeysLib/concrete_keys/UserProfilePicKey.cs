using SharedKernel.errs.utils;

namespace VokimiStorageKeysLib.users;

public class UserProfilePicKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public AppUserId UserId { get; }

    public UserProfilePicKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, Scheme.IsKeyValid(value, out var userId));
        UserId = userId;
        Value = value;
    }

    public static ErrOr<UserProfilePicKey> CreateNewForUser(AppUserId userId, string extension) {
        if (!AllowedExtensions.Contains(extension)) {
            return ErrFactory.IncorrectFormat(
                $"Incorrect extension. Expected {string.Join(", ", AllowedExtensions)}, found: '{extension}'"
            );
        }

        return new UserProfilePicKey($"user-profile-pics/{userId}/{Guid.NewGuid()}.{extension}");
    }

    public bool IsForUser(AppUserId userId) => UserId == userId;

    private class Scheme
    {
        private const string Template = $"{KeyConsts.UserProfilePicsFolder}/<userId:id>";
        private static readonly KeyTemplateParser Parser = new(Template, AllowedExtensions);

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
}