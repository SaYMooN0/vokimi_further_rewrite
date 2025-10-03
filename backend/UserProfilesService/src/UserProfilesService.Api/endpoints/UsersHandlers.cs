using SharedKernel.common.app_users;
using UserProfilesService.Api.contracts;
using UserProfilesService.Application.app_users.queries;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Api.endpoints;

public static class UsersHandlers
{
    internal static void MapUsersHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/users/");

        group.MapPost("/preview", GetUserPreviewData)
            .WithRequestValidation<UsersPreviewRequest>();
    }

    private static async Task<IResult> GetUserPreviewData(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListUsersNamesWithProfilePicsQuery,
            Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>> handler
    ) {
        var request = httpContext.GetValidatedRequest<UsersPreviewRequest>();

        ListUsersNamesWithProfilePicsQuery query = new(request.ParsedUserIds);
        var result = await handler.Handle(query, ct);

        return CustomResults
            .FromErrOrToJson<
                Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>,
                MultipleUsersPreviewResponse
            >(result);
    }
}