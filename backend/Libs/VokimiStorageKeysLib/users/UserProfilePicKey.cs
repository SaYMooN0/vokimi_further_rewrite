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

    public bool IsForUser(AppUserId userId) => UserId == userId;
}