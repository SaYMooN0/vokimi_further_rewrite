using UserProfilesService.Api.contracts;
using UserProfilesService.Application.app_users.queries;
using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Api.endpoints;

public static class UsersHandlers
{
    internal static void MapUsersHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/users/");

        group.MapPost("/preview", GetUserPreviewData)
            .WithRequestValidation<UsersPreviewRequest>();
        
        group.MapGet("/search-to-invite", SearchUsersToInviteByName);

    }

    private static async Task<IResult> GetUserPreviewData(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListUsersNamesWithProfilePicsQuery, UserPreviewDto[]> handler
    ) {
        var request = httpContext.GetValidatedRequest<UsersPreviewRequest>();

        ListUsersNamesWithProfilePicsQuery query = new(request.ParsedUserIds);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserPreviewDto[], MultipleUsersPreviewResponse>(result);
    }
    private static async Task<IResult> SearchUsersToInviteByName(
        string searchValue, int limit,
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<SearchUsersByNameQuery, UserPreviewWithAllowInvitesSettingDto[]> handler
    ) {
        SearchUsersByNameQuery query = new(searchValue, limit);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserPreviewWithAllowInvitesSettingDto[], ListUsersToInviteResponse>(result);
    }
}   