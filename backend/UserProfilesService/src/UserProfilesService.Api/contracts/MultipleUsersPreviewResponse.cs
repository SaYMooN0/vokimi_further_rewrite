using SharedKernel.common.app_users;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Api.contracts;

public record class MultipleUsersPreviewResponse(
    Dictionary<string, UserNameWithProfilePicResponse> Users
) : ICreatableResponse<Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>>
{
    public static ICreatableResponse<Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>> Create(
        Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)> users
    ) => new MultipleUsersPreviewResponse(
        users.ToDictionary(
            u => u.Key.ToString(),
            u => UserNameWithProfilePicResponse.FromTuple(u.Value)
        )
    );
}

public record UserNameWithProfilePicResponse(string Name, string ProfilePic)
{
    public static UserNameWithProfilePicResponse FromTuple((AppUserName Name, UserProfilePicKey PicKey) tuple) => new(
        tuple.Name.ToString(),
        tuple.PicKey.ToString()
    );
}