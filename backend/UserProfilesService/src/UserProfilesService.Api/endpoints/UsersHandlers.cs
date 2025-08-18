using UserProfilesService.Api.contracts;
using UserProfilesService.Application.app_users.queries;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.endpoints;

public static class UsersHandlers
{
    internal static void MapUsersHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/users/{userId}");

        group.MapGet("/preview", GetUserPreviewData);
    }

    private static async Task<IResult> GetUserPreviewData(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetUserQuery, AppUser> handler
    ) {
        AppUserId userId = httpContext.GetAppUserIdFromRoute();

        GetUserQuery query = new(userId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (user) => Results.Json(
            UserPreviewResponse.Create(user)
        ));
    }
}