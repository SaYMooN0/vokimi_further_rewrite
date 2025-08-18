using SharedKernel.errs.utils;

namespace VokimiStorageKeysLib.users;

public class UserProfilePicKey : BaseStorageKey
{
    protected override string Value { get; }
    public AppUserId UserId { get; }

    public UserProfilePicKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, UserProfilePicKeyScheme.IsKeyValid(
            value, out var userId
        ));
        UserId = userId;
        Value = value;
    }

    public static ErrOr<UserProfilePicKey> CreateNewForUser(AppUserId userId, string extension) {
        if (!Extensions.ImageFiles.Contains(extension)) {
            return ErrFactory.IncorrectFormat(
                $"Incorrect extension. Expected {string.Join(", ", Extensions.ImageFiles)}, found: '{extension}'"
            );
        }

        return new UserProfilePicKey($"user-profile-pics/{userId}/{Guid.NewGuid()}.{extension}");
    }

    public bool IsForUser(AppUserId userId) => UserId == userId;
}