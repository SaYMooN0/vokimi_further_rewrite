using UserProfilesService.Domain.app_user_aggregate.dtos;

namespace UserProfilesService.Api.contracts;

public record class UserProfileViewResponse() : ICreatableResponse<UserProfileViewDto>
{
    public static ICreatableResponse<UserProfileViewDto> Create(UserProfileViewDto d) =>
        throw new NotImplementedException();
}