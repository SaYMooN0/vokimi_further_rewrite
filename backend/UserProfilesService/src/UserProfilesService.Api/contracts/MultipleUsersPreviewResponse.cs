using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Api.contracts;

public record class MultipleUsersPreviewResponse(
    Dictionary<string, UserNameWithProfilePicResponse> Users
) : ICreatableResponse<UserPreviewDto[]>
{
    public static ICreatableResponse<UserPreviewDto[]> Create(UserPreviewDto[] users) =>
        new MultipleUsersPreviewResponse(
            users.ToDictionary(
                u => u.UserId.ToString(),
                UserNameWithProfilePicResponse.FromDto
            )
        );
}

public record UserNameWithProfilePicResponse(string UniqueName, string DisplayName, string ProfilePic)
{
    public static UserNameWithProfilePicResponse FromDto(UserPreviewDto dto) => new(
        dto.UniqueName.ToString(), dto.DisplayName.ToString(), dto.ProfilePicKey.ToString()
    );
}